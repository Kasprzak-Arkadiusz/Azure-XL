using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Websites.Commands;

public class AddWebsitesCommand : IRequest<Unit>
{
    public List<AddWebsiteRequest> Websites { get; }

    public AddWebsitesCommand(List<AddWebsiteRequest> websites)
    {
        Websites = websites;
    }
}

public class AddWebsitesCommandHandler : IRequestHandler<AddWebsitesCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IKeyPhraseExtractor _keyPhraseExtractor;

    public AddWebsitesCommandHandler(IApplicationDbContext dbContext,
        IKeyPhraseExtractor keyPhraseExtractor)
    {
        _dbContext = dbContext;
        _keyPhraseExtractor = keyPhraseExtractor;
    }

    public async Task<Unit> Handle(AddWebsitesCommand query, CancellationToken cancellationToken)
    {
        var newRequestedWebsites = await GetNewWebsitesAsync(query.Websites, cancellationToken);
        if (newRequestedWebsites is null)
        {
            return Unit.Value;
        }

        var websitePhraseNames = await AssociateWebsitesWithKeyPhrasesAsync(newRequestedWebsites, cancellationToken);
        var allPhrases = await GetAllKeyPhrasesAsync(websitePhraseNames, cancellationToken);

        foreach (var (website, phrases) in websitePhraseNames)
        {
            var websiteKeyPhraseList = allPhrases.Where(p => phrases.Contains(p.Name));
            website.SetKeyPhrases(websiteKeyPhraseList.Select(p => WebsiteKeyPhrase.Create(website, p)).ToList());
            _dbContext.Websites.Add(website);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<List<KeyPhrase>> GetAllKeyPhrasesAsync(
        IEnumerable<(Website website, List<string> phrases)>? websitePhraseNames, CancellationToken cancellationToken)
    {
        if (websitePhraseNames is null)
        {
            return new List<KeyPhrase>();
        }

        var uniqueExtractedPhrases = websitePhraseNames.SelectMany(wpn => wpn.phrases).Select(KeyPhrase.Create)
            .Distinct().ToList();
        var existingPhrases = await _dbContext.KeyPhrases
            .Where(kp => uniqueExtractedPhrases.Select(up => up.Name).Contains(kp.Name))
            .ToListAsync(cancellationToken);
        var newPhrases = uniqueExtractedPhrases.Except(existingPhrases).ToList();
        _dbContext.KeyPhrases.AddRange(newPhrases);

        var allPhrasesCount = newPhrases.Count + existingPhrases.Count;
        var allPhrases = new List<KeyPhrase>(allPhrasesCount);
        allPhrases.AddRange(newPhrases);
        allPhrases.AddRange(existingPhrases);

        return allPhrases;
    }

    private async Task<List<(Website website, List<string> phrases)>> AssociateWebsitesWithKeyPhrasesAsync(
        List<AddWebsiteRequest> newRequestedWebsites, CancellationToken cancellationToken)
    {
        var websitePhraseNames = new List<(Website website, List<string> phrases)>();
        foreach (var requestedWebsite in newRequestedWebsites)
        {
            var url = requestedWebsite.Link;
            var title = requestedWebsite.Title;
            var extractedPhrases = await _keyPhraseExtractor.ExtractKeyPhrasesAsync(title, cancellationToken);
            websitePhraseNames.Add((Website.Create(url, title), extractedPhrases.ToList()));
        }

        return websitePhraseNames;
    }

    private async Task<List<AddWebsiteRequest>?> GetNewWebsitesAsync(ICollection<AddWebsiteRequest> requestedWebsites,
        CancellationToken cancellationToken)
    {
        var existingUrls = await _dbContext.Websites.Where(w => requestedWebsites.Select(rw => rw.Link).Contains(w.Url))
            .Select(w => w.Url)
            .ToListAsync(cancellationToken);

        return requestedWebsites.Where(rw => !existingUrls.Contains(rw.Link)).ToList();
    }
}
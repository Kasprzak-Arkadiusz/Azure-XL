using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Websites;

public class AddKeyPhrasesToWebsitesCommand : IRequest<Unit>
{
    public AddKeyPhrasesToWebsitesCommand() { }
}

public class AddKeyPhrasesToWebsitesCommandHandler : IRequestHandler<AddKeyPhrasesToWebsitesCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IKeyPhraseExtractor _keyPhraseExtractor;

    public AddKeyPhrasesToWebsitesCommandHandler(IApplicationDbContext dbContext,
        IKeyPhraseExtractor keyPhraseExtractor)
    {
        _dbContext = dbContext;
        _keyPhraseExtractor = keyPhraseExtractor;
    }

    public async Task<Unit> Handle(AddKeyPhrasesToWebsitesCommand query, CancellationToken cancellationToken)
    {
        var websitesWithoutPhrases = await _dbContext.Websites.Where(w => w.WebsiteKeyPhrases.Count == 0)
            .ToListAsync(cancellationToken);

        var websitePhrases = new List<(Website website, List<KeyPhrase> phrases)>();
        foreach (var website in websitesWithoutPhrases)
        {
            var extractedPhrases = await _keyPhraseExtractor.ExtractKeyPhrasesAsync(website.Title, cancellationToken);
            websitePhrases.Add((website, extractedPhrases.Select(KeyPhrase.Create).ToList()));
        }

        // var newPhrases = await GetNewPhrasesAsync(websitePhrases.SelectMany(up => up.phrases), cancellationToken);
        // _dbContext.KeyPhrases.AddRange(newPhrases);
        // await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var websitePhrase in websitePhrases)
        {
            var websiteKeyPhraseList = websitePhrase.phrases
                .Select(phrase => WebsiteKeyPhrase.Create(websitePhrase.website, phrase)).ToList();
            _dbContext.WebsiteKeyPhrases.AddRange(websiteKeyPhraseList);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private async Task<IEnumerable<KeyPhrase>> GetNewPhrasesAsync(IEnumerable<KeyPhrase> foundPhrases,
        CancellationToken cancellationToken)
    {
        var uniquePhrases = foundPhrases.Distinct().ToList();
        var existingPhrases = await _dbContext.KeyPhrases
            .Where(kp => uniquePhrases.Contains(kp))
            .ToListAsync(cancellationToken);

        return uniquePhrases.Except(existingPhrases);
    }
}
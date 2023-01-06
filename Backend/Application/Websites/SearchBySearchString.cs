using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Websites;

public class SearchBySearchString : IRequest<WebsiteSearchResult>
{
    public string SearchString { get; init; }

    public SearchBySearchString(string searchString)
    {
        SearchString = searchString;
    }
}

public class SearchBySearchStringHandler : IRequestHandler<SearchBySearchString, WebsiteSearchResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IKeyPhraseExtractor _keyPhraseExtractor;

    public SearchBySearchStringHandler(IApplicationDbContext dbContext, IKeyPhraseExtractor keyPhraseExtractor)
    {
        _dbContext = dbContext;
        _keyPhraseExtractor = keyPhraseExtractor;
    }

    public async Task<WebsiteSearchResult> Handle(SearchBySearchString query, CancellationToken cancellationToken)
    {
        var searchedPhrases = await _keyPhraseExtractor.ExtractKeyPhrasesAsync(query.SearchString, cancellationToken);

        var website = _dbContext.WebsiteKeyPhrases
            .Include(wkp => wkp.Website)
            .Where(wkp => searchedPhrases.Contains(wkp.KeyPhraseName))
            .GroupBy(wkp => new { wkp.WebsiteUrl, wkp.Website.Title })
            .Select(wkp => new
            {
                Url = wkp.Key.WebsiteUrl,
                Title = wkp.Key.Title,
                Count = wkp.Count()
            })
            .OrderByDescending(w => w.Count)
            .First();

        return new WebsiteSearchResult
        {
            Url = website.Url,
            Title = website.Title
        };
    }
}
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Websites;

public class SearchBySearchStringQuery : IRequest<WebsiteSearchResult>
{
    public string SearchString { get; init; }

    public SearchBySearchStringQuery(string searchString)
    {
        SearchString = searchString;
    }
}

public class SearchBySearchStringHandler : IRequestHandler<SearchBySearchStringQuery, WebsiteSearchResult>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IKeyPhraseExtractor _keyPhraseExtractor;

    public SearchBySearchStringHandler(IApplicationDbContext dbContext, IKeyPhraseExtractor keyPhraseExtractor)
    {
        _dbContext = dbContext;
        _keyPhraseExtractor = keyPhraseExtractor;
    }

    public async Task<WebsiteSearchResult> Handle(SearchBySearchStringQuery query, CancellationToken cancellationToken)
    {
        var searchedPhrases = await _keyPhraseExtractor.ExtractKeyPhrasesAsync(query.SearchString, cancellationToken);

        var website = await _dbContext.WebsiteKeyPhrases
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
            .FirstOrDefaultAsync(cancellationToken);

        if (website is null)
        {
            throw new NotFoundException("Nie znaleziono pasujących wyników");
        }
        
        return new WebsiteSearchResult
        {
            Url = website.Url,
            Title = website.Title
        };
    }
}
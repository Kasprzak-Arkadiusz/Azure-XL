using Application.Common.Interfaces;
using Azure;
using Azure.AI.TextAnalytics;
using Infrastructure.Common.Exceptions;

namespace Infrastructure.Services;

public class KeyPhraseExtractor : IKeyPhraseExtractor
{
    private static readonly AzureKeyCredential Credentials = new("INSERT-HERE-YOUR-CREDENTIALS");
    private static readonly Uri Endpoint = new("INSERT-HERE-YOUR-ENDPOINT");
    private const string SplitSeparator = " ";

    private readonly TextAnalyticsClient _textAnalyticsClient;

    public KeyPhraseExtractor()
    {
        _textAnalyticsClient = new TextAnalyticsClient(Endpoint, Credentials);
    }

    public async Task<List<string>> ExtractKeyPhrasesAsync(string text, CancellationToken cancellationToken)
    {
        var response = await _textAnalyticsClient.ExtractKeyPhrasesAsync(text, null, cancellationToken);
        if (response is null)
        {
            throw new ExternalProviderException("Odpowiedź od Cognitive Service jest nullem");
        }

        if (!response.HasValue)
        {
            return new List<string>();
        }

        var splitKeyPhrases = SplitKeyPhrases(response.Value.ToList());
        return  splitKeyPhrases.Select(keyPhrase => keyPhrase.ToLower()).ToList();
    }

    private static IEnumerable<string> SplitKeyPhrases(IEnumerable<string> keyPhrases)
    {
        return keyPhrases.SelectMany(kp => kp.Split(SplitSeparator)).Distinct().ToList();
    }
}
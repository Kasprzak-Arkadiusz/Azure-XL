using Application.Common.Interfaces;
using Azure;
using Azure.AI.TextAnalytics;
using Infrastructure.Common.Exceptions;

namespace Infrastructure.Services;

public class KeyPhraseExtractor : IKeyPhraseExtractor
{
    private static readonly AzureKeyCredential Credentials = new("replace-with-your-key-here");
    private static readonly Uri Endpoint = new("replace-with-your-endpoint-here");
    private const string SplitSeparator = "";

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

        return response.HasValue ? SplitKeyPhrases(response.Value.ToList()) : new List<string>();
    }

    private static List<string> SplitKeyPhrases(IEnumerable<string> keyPhrases)
    {
        return keyPhrases.SelectMany(kp => kp.Split(SplitSeparator)).Distinct().ToList();
    }
}
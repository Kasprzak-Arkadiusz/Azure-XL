namespace Application.Common.Interfaces;

public interface IKeyPhraseExtractor
{
    Task<List<string>> ExtractKeyPhrasesAsync(string text, CancellationToken cancellationToken = new CancellationToken());
}
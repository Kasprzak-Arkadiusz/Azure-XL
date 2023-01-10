namespace Domain.Entities;

public class Website
{
    public string Url { get; private set; }
    public string Title { get; private set; }
    public byte[]? Image { get; private set; }

    public List<WebsiteKeyPhrase> WebsiteKeyPhrases { get; private set; }

    private Website() { }

    private Website(string url, string title, IEnumerable<KeyPhrase>? keyPhrases, byte[]? image)
    {
        Url = url;
        Title = title;
        WebsiteKeyPhrases = keyPhrases is null
            ? new List<WebsiteKeyPhrase>()
            : keyPhrases.Select(kp => WebsiteKeyPhrase.Create(this, kp)).ToList();
        Image = image;
    }

    public static Website Create(string url, string title, IEnumerable<KeyPhrase>? keyPhrases = null,
        byte[]? image = null)
    {
        return new Website(url, title, keyPhrases, image);
    }

    public void SetKeyPhrases(List<WebsiteKeyPhrase> keyPhrases)
    {
        WebsiteKeyPhrases = keyPhrases;
    }
}
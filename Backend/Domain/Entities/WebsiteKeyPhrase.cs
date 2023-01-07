namespace Domain.Entities;

public class WebsiteKeyPhrase
{
    public string KeyPhraseName { get; private set; }
    public KeyPhrase KeyPhrase { get; private set; }
    public string WebsiteUrl { get; private set; }
    public Website Website { get; private set; }

    public WebsiteKeyPhrase() { }

    private WebsiteKeyPhrase( Website website, KeyPhrase keyPhrase)
    {
        Website = website;
        WebsiteUrl = website.Url;
        KeyPhrase = keyPhrase;
        KeyPhraseName = keyPhrase.Name;
    }

    public static WebsiteKeyPhrase Create( Website website, KeyPhrase keyPhrase)
    {
        return new WebsiteKeyPhrase(website, keyPhrase);
    }
}
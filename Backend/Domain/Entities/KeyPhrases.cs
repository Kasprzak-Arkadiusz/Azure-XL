namespace Domain.Entities;

public class KeyPhrase
{
    public string Name { get; private set; }

    public List<WebsiteKeyPhrase> WebsitesKeyPhrases { get; private set; }

    private KeyPhrase() { }

    private KeyPhrase(string name)
    {
        Name = name;
    }

    public static KeyPhrase Create(string name)
    {
        return new KeyPhrase(name);
    }
}
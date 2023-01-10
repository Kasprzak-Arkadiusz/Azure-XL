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

    public override bool Equals(object? obj)
    {
        return obj is KeyPhrase phrase && string.Equals(Name, phrase.Name, StringComparison.CurrentCultureIgnoreCase);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
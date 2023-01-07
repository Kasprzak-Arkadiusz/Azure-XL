namespace Infrastructure.Settings;

public class InfrastructureSettings
{
    public string DbConnectionString { get; set; } = null!;
    public string KeyVaultUri { get; set; }
}
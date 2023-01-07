using Application.Common.Interfaces;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.Settings;

namespace Infrastructure.Services;

public class AzureSecretSupplier : ISecretSupplier
{
    private readonly SecretClient _secretClient;

    public AzureSecretSupplier(InfrastructureSettings settings)
    {
        var options = new SecretClientOptions()
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential
            }
        };

        _secretClient = new SecretClient(new Uri(settings.KeyVaultUri), new DefaultAzureCredential(), options);
    }

    public string GetSecret(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return string.Empty;
        }

        KeyVaultSecret secret = _secretClient.GetSecret(key);
        return secret.Value;
    }
}
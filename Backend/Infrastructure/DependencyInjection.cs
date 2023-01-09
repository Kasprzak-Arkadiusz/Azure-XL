using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, InfrastructureSettings settings)
    {
        services.AddSingleton(settings);
        services.AddSingleton<ISecretSupplier, AzureSecretSupplier>();

        services.AddTransient<IKeyPhraseExtractor, KeyPhraseExtractor>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseSqlServer(settings.DbConnectionString)
        );
    }
}
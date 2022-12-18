using Microsoft.Extensions.Options;

namespace Infrastructure.Settings;

public class InfrastructureSettingsValidator : IValidateOptions<InfrastructureSettings>
{
    private const string MissingSettingMessage = "InfrastructureSettings: \"{0}\" is missing in appsettings";

    public ValidateOptionsResult Validate(string name, InfrastructureSettings options)
    {
        var connectionString = options.DbConnectionString;
        return string.IsNullOrEmpty(connectionString)
            ? ValidateOptionsResult.Fail(string.Format(MissingSettingMessage, nameof(options.DbConnectionString)))
            : ValidateOptionsResult.Success;
    }
}
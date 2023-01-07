using System.Reflection;
using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class SwaggerDocExtension
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Azure-XL",
                    Description = "An ASP.NET Web API for Azure project related to the search engine"
                }
            );

            opt.SupportNonNullableReferenceTypes();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}
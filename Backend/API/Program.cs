using API.Extensions;
using Infrastructure;
using Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var infrastructureSettings = new InfrastructureSettings();
builder.Configuration.Bind(nameof(InfrastructureSettings), infrastructureSettings);
builder.Services.AddInfrastructure(infrastructureSettings);

// Add services to the container.
const string corsPolicyName = "FrontendPolicy";
builder.Services.AddCors(o => o.AddPolicy(corsPolicyName, corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocumentation();
builder.Services.AddSwaggerGen(options => { options.CustomSchemaIds(type => type.FullName); });

var app = builder.Build();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.InjectStylesheet("/swagger-ui/style.css"); });
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
using ChampionBuildApi.Services;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // General information
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mobascraper",
        Version = "v1",
        Description = "An API for retrieving champion builds and scraping data from Mobalytics.",
    });

    // If you're using specific API models, make sure to include them here
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "YourXmlDocumentation.xml"));
});

builder.Services.AddScoped<IChampionScraper, ChampionScraper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();  // Enable Swagger UI middleware
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mobascraper V1");
    c.RoutePrefix = string.Empty; // Optional: Makes Swagger UI available at the root URL
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

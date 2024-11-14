using ChampionBuildApi.Services;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models; // Add this using directive

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
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register the IChampionScraper service
builder.Services.AddScoped<IChampionScraper, ChampionScraper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => // Modify this line to include a parameter
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mobascraper V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

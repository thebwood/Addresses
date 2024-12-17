using Addresses.Api.Extensions;
using Addresses.DatabaseLayer.Extensions;
using Addresses.BusinessLayer.Extensions;
string siteCorsPolicy = "SiteCorsPolicy";

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(siteCorsPolicy,
                           builder =>
                           {
                               builder.WithOrigins("https://localhost:7027", "https://localhost:7292")
                                                   .AllowAnyHeader()
                                                   .AllowAnyMethod()
                                                   .AllowCredentials();
                           });
    });

    builder.Services
        .AddPresentation()
        .AddDatabaseLayer(builder.Configuration.GetConnectionString("Database"))
        .AddBusinessLayer();
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

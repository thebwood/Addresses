using Addresses.Api.Extensions;
using Addresses.DatabaseLayer.Extensions;
using Addresses.BusinessLayer.Extensions;
using Addresses.Api.Middlewares;

string siteCorsPolicy = "SiteCorsPolicy";

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(siteCorsPolicy,
                           builder =>
                           {
                               builder.WithOrigins("https://localhost:7027", "https://localhost:7292", "https://localhost:7239")
                                                   .AllowAnyHeader()
                                                   .AllowAnyMethod()
                                                   .AllowCredentials();
                           });
    });
    builder.Services.AddSwaggerGen();


    builder.Services
        .AddPresentation()
        .AddDatabaseLayer(builder.Configuration.GetConnectionString("Database"))
        .AddBusinessLayer();
}

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Add the GlobalExceptionMiddleware to the request pipeline.
app.UseMiddleware<GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}
app.UseCors(siteCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();

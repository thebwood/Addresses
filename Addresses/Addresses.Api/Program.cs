using Addresses.Api.Extensions;
using Addresses.DatabaseLayer.Extensions;
using Addresses.BusinessLayer.Extensions;
using Addresses.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Addresses.Domain.Common;
using System.IdentityModel.Tokens.Jwt;

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

    // Add JWT Authentication
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
        {
            // Check if the token is blacklisted
            var token = securityToken as JwtSecurityToken;
            if (token != null)
            {
                var tokenId = token.RawData;
                // Implement logic to check if the token is blacklisted
                return !IsTokenBlacklisted(tokenId);
            }
            return false;
        }
    };
});

}

bool IsTokenBlacklisted(string tokenId)
{
    throw new NotImplementedException();
}

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(siteCorsPolicy);

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();

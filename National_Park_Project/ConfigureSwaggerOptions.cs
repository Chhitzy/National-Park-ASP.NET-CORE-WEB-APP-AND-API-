using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;

namespace National_Park_Project
{
    public class ConfigureSwaggerOptions:IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "I am too lazy to type ZZzzz...." + "Name :Authorization \r \n" + "Jn: Header", Name = "Authorization", In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                },
                Scheme  = "Oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
                }
            });
        
        }
    }
}

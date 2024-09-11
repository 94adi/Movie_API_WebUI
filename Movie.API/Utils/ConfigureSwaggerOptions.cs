﻿using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movie.API.Utils;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) 
    : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
           "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
           "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
           "Example: \"Bearer 12345abcdef\"",
            Name = "JWT Auth",
            In = ParameterLocation.Header,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(desc.GroupName, 
                new OpenApiInfo
                {
                    Version = desc.ApiVersion.ToString(),
                    Title = $"Movie API {desc.ApiVersion}",
                    Description = "API to manage Movies Database",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Adrian Cotuna",
                        Email = "adrianrcotuna@gmail.com",
                        Url = new Uri("https://www.linkedin.com/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });
        }
    }
}

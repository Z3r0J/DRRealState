using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DRRealState.WebApi.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddSwaggerExtension(this IServiceCollection service) {

            service.AddSwaggerGen(options => {

                List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();

                xmlFiles.ForEach(xmlFiles => options.IncludeXmlComments(xmlFiles));

                options.SwaggerDoc("v1", new OpenApiInfo { 
                
                    Version="v1",
                    Title = "DRRealState API",
                    Description = "This API is created for services using in a Real Estate.",
                    Contact = new OpenApiContact { 
                    Name = "Jean Carlos Reyes, Johanly Baez Lima, Jose Miguel Cayetano Marquez",
                    Email = "jeanrey.ese@gmail.com",
                    Url = new Uri("https://github.com/z3r0j/DRRealState")
                    }

                });
                options.EnableAnnotations();
                options.DescribeAllParametersInCamelCase();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name="Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format = Bearer {your token here}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    { 
                    new OpenApiSecurityScheme{
                        
                        Reference = new OpenApiReference{ 
                        Type= ReferenceType.SecurityScheme,
                        Id="Bearer"
                        },
                        Scheme ="Bearer",
                        Name="Bearer",
                        In= ParameterLocation.Header,
                    
                    },
                
                        new List<string>()
                    }
                });

            });

        }

        public static void AddApiVersioningExtension(this IServiceCollection services) {

            services.AddApiVersioning(config => {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            
            });

        }
    }
}

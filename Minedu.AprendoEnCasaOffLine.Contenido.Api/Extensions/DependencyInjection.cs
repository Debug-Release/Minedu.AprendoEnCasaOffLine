using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minedu.AprendoEnCasaOffLine.Contenido.Core;
using System.Text;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicacion(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings() { Settings = new Setting(), Token = new TokenSetting() };
            configuration.GetSection("Settings").Bind(appSettings.Settings);
            configuration.GetSection("Token").Bind(appSettings.Token);
            services.AddSingleton(appSettings);


            /*IS4
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIS4Bearer();
            */

            #region Configure jwt authentication

            var ts = appSettings.Token;
            var key = Encoding.ASCII.GetBytes(ts.TOKEN_SECRET);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userName = context.Principal.Identity.Name;
                        if (!(userName == ts.TOKEN_USER))
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    //RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            #endregion

            /*
            #region WorkerService

            services.AddHostedService<ProgramacionService>();

            #endregion
            */
            //services.AddMediatR(typeof(Startup));

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                /*
                var provider = services.BuildServiceProvider();
                var service = provider.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (ApiVersionDescription item in service.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(item.GroupName, CreateMetaInfoAPIVersion(item));
                }
                */
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "[Minedu Aprendo-en-Casa Contenido API]" + " v1",
                        Description = "[Minedu Aprendo-en-Casa Contenido API] - Gestionar descargas de contenido programado para servidores",
                        Contact = new OpenApiContact
                        {
                            Name = "MINEDU"
                        }
                    });
                c.SwaggerDoc("v2",
                    new OpenApiInfo
                    {
                        Version = "v2",
                        Title = "[Minedu Aprendo-en-Casa Contenido API]" + " v2",
                        Description = "[Minedu Aprendo-en-Casa Contenido API] - Gestionar descargas de contenido programado para servidores",
                        Contact = new OpenApiContact
                        {
                            Name = "MINEDU"
                        }
                    });
                c.EnableAnnotations();
                c.CustomSchemaIds(x => x.FullName);
            });
            services.AddSwaggerGenNewtonsoftSupport();


            #endregion

            #region Version API

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            /*
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            */           

            #endregion

            #region Upload

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            #endregion

            services.AddCustomCors();

            return services;
        }

        private static OpenApiInfo CreateMetaInfoAPIVersion(ApiVersionDescription apiDescription)
        {
            //var v = apiDescription.ApiVersion.ToString();
            return new OpenApiInfo
            {
                Title = "[Minedu Aprendo-en-Casa Contenido API]" + $" {apiDescription.GroupName}",
                Version = apiDescription.GroupName,
                Description = "[Minedu Aprendo-en-Casa Contenido API] - Gestionar descargas de contenido programado para servidores",
                Contact = new OpenApiContact
                {
                    Name = "MINEDU"
                }
            };
        }
    }
}

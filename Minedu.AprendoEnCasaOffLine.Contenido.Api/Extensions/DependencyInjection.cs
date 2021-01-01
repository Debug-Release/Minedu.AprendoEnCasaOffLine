using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            services.AddMediatR(typeof(Startup));

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "[Minedu Aprendo-en-Casa Contenido API]",
                        Description = "[Minedu Aprendo-en-Casa Contenido API] - Gestionar descargas de contenido programado para servidores",
                        Version = "V1",
                        Contact = new OpenApiContact
                        {
                            Name = "MINEDU"
                        }
                    });
                c.EnableAnnotations();
            });

            #endregion

            return services;
        }
    }
}

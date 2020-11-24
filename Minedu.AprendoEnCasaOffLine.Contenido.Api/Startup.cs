using Autofac;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minedu.AprendoEnCasaOffLine.Contenido.Api.Models;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Filters;
using Minedu.AprendoEnCasaOffLine.Contenido.Worker;
using Minedu.IS4.Security.Auth;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                DotNetEnv.Env.Load();
            }

            SecurityConfig.Init(Configuration);
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddAuthorization();

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<ValidationFilter>();
            })
            .AddFluentValidation(fv => { fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; })
            .AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            /*IS4
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddIS4Bearer();
            */

            #region Configure jwt authentication

            var ts = new TokenSettings();
            var key = Encoding.ASCII.GetBytes(ts.Secret);
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
                        if (!(userName == ts.User))
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

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //Register Types
            BootstrapperContainer.Configuration = Configuration;
            BootstrapperContainer.Register(builder);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            Directory.SetCurrentDirectory(env.ContentRootPath);


            //IS4
            //app.UseAuthentication();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            string color = env.IsDevelopment() ? "Gray" : "Green";
            string href = $"<a href='/swagger'>{env.EnvironmentName}</a>";

            app.Run(context =>
                context.Response.WriteAsync($"<h1 style='color:{color};'>[MS.Api] Environment: {href}</h1>")
            );

        }
    }
}

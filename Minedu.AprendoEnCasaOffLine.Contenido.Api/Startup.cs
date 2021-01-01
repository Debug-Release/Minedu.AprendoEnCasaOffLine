using Autofac;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Minedu.AprendoEnCasaOffLine.Contenido.Core.Filters;
using Minedu.IS4.Security.Auth;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;

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

            services.AddAplicacion(Configuration);

        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            BootstrapperContainer.Register(builder, Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            Directory.SetCurrentDirectory(env.ContentRootPath);


            //IS4
            //app.UseAuthentication();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.RoutePrefix = "swagger";
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            string color = env.IsDevelopment() ? "Gray" : "Green";
            string href = $"<a href='/swagger'>{env.EnvironmentName}</a>";

            app.Run(context =>
                context.Response.WriteAsync($"<h1 style='color:{color};'>[MS.Api] Environment: {href}</h1>")
            );

        }
    }
}

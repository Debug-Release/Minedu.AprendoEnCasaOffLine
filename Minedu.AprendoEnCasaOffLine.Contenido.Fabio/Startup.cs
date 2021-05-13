using Convey;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Fabio
{
    public class JaegerOptions
    {
        public bool Enabled { get; set; }
        public string ServiceName { get; set; }
        public string UdpHost { get; set; }
        public int UdpPort { get; set; }
        public int MaxPacketSize { get; set; }
        public string Sampler { get; set; }
        public double MaxTracesPerSecond { get; set; } = 5;
        public double SamplingRate { get; set; } = 0.2;
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddConvey()
                .RegisterConvey(Configuration);

            /*
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                string serviceName = Assembly.GetEntryAssembly().GetName().Name;

                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                var options = GetJaegerOptions(services);

                var reporter = new RemoteReporter
                       .Builder()
                   .WithSender(new UdpSender(options.UdpHost, options.UdpPort, options.MaxPacketSize))
                   .WithLoggerFactory(loggerFactory)
                   .Build();

                var sampler = GetSampler(options);

                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();
            */
            services.AddControllers();


        }

        private static ISampler GetSampler(JaegerOptions options)
        {
            switch (options.Sampler)
            {
                case "const": return new ConstSampler(true);
                case "rate": return new RateLimitingSampler(options.MaxTracesPerSecond);
                case "probabilistic": return new ProbabilisticSampler(options.SamplingRate);
                default: return new ConstSampler(true);
            }
        }
        private static JaegerOptions GetJaegerOptions(IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<JaegerOptions>(configuration.GetSection("jaeger"));
                return configuration.GetOptions<JaegerOptions>("jaeger");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseConvey();

            //app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*
            var consulServiceId = app.UseConsul();
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                client.Agent.ServiceDeregister(consulServiceId);                
            });
            */
            string color = env.IsDevelopment() ? "Gray" : "Green";

            app.Run(context =>
                context.Response.WriteAsync($"<h1 style='color:{color};'>[MS.Api] Environment</h1>")
            );
        }
    }
}

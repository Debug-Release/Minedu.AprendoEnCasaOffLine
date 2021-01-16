using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Release.Helper;
using System.Collections.Generic;

namespace Minedu.IS4.Security.Auth
{

    public class SecuritySettings
    {
        public string[] CORSDomains { get; set; }
    }

    public static class SecurityConfig
    {
        public static IConfiguration Configuration;

        public static void Init(IConfiguration config)
        {
            Configuration = config;
        }

        private static SecuritySettings _settings;

        public static SecuritySettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    var ASPNETCORE_CORS = Functions.GetEnvironmentVariable("ASPNETCORE_CORS");

                    var vv = new SecuritySettings
                    {
                        CORSDomains = ASPNETCORE_CORS?.Split(new char[] { ',', ';' })
                    };


                    var arrCorsDomains = vv.CORSDomains;
                    var lstCorsDomains = new List<string>();
                    if (arrCorsDomains != null && arrCorsDomains.Length > 0)
                    {
                        foreach (var domain in arrCorsDomains)
                        {
                            if (string.IsNullOrWhiteSpace(domain)) continue;
                            lstCorsDomains.Add(domain.Trim());
                        }
                    }

                    vv.CORSDomains = lstCorsDomains.ToArray();

                    _settings = vv;
                }

                return _settings;
            }


        }

        public static CorsPolicy GetCORSPolicy()
        {
            var corsPolicyBuilder = new CorsPolicyBuilder();
            corsPolicyBuilder.AllowAnyMethod();
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowCredentials();

            if (Settings.CORSDomains != null && Settings.CORSDomains.Length > 0)
            {
                corsPolicyBuilder.WithOrigins(Settings.CORSDomains);
            }
            else
            {
                corsPolicyBuilder.AllowAnyOrigin();
            }
            return corsPolicyBuilder.Build();
        }        
    }
}

using System;
using AutoMapper;

using Lykke.Sdk;
using Lykke.Service.ClientDialogs.Middleware;
using Lykke.Service.ClientDialogs.Profiles;
using Lykke.Service.ClientDialogs.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Lykke.Service.ClientDialogs
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(typeof(ServiceProfile));
            });

            Mapper.AssertConfigurationIsValid();

            return services.BuildServiceProvider<AppSettings>(options =>
            {
                options.ApiTitle = "ClientDialogs API";
                options.Logs = ("ClientDialogsLog", ctx => ctx.ClientDialogsService.Db.LogsConnString);
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory logger)
        {
            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseLykkeConfiguration();

            logger.AddConsole();
            logger.AddDebug();
        }
    }
}

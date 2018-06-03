using System;
using AutoMapper;
using Lykke.Sdk;
using Lykke.Service.ClientDialogs.Profiles;
using Lykke.Service.ClientDialogs.Settings;
using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
                options.LogsConnectionStringFactory = ctx => ctx.ConnectionString(x => x.ClientDialogsService.Db.LogsConnString);
                options.LogsTableName = "ClientDialogsLog";
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();
        }
    }
}

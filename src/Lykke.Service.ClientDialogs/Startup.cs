﻿using System;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.ClientDialogs.Profiles;
using Lykke.Service.ClientDialogs.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.ClientDialogs
{
    [UsedImplicitly]
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
                options.Logs = loggingOptions =>
                {
                    loggingOptions.AzureTableName = "ClientDialogsLog";
                    loggingOptions.AzureTableConnectionStringResolver =
                        settings => settings.ClientDialogsService.Db.LogsConnString;
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration();
        }
    }
}

using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit;
using Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Services;
using Lykke.Service.ClientDialogs.Settings;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.ClientDialogs.Modules
{    
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;
        private readonly ILog _log;
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<AppSettings> appSettings, ILog log)
        {
            _appSettings = appSettings;
            _log = log;
            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(
                new ClientDialogsRepository(AzureTableStorage<ClientDialogEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "Dialogs", _log),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "ClientDialogs", _log),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "GlobalDialogs", _log)
                    )
            ).As<IClientDialogsRepository>().SingleInstance();
            
            builder.RegisterInstance(
                new ClientDialogSubmitsRepository(AzureTableStorage<ClientDialogSubmitEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "SubmittedDialogs",
                   _log))
            ).As<IClientDialogSubmitsRepository>().SingleInstance();
            
            builder.RegisterInstance(
                new DialogConditionsRepository(AzureTableStorage<DialogConditionEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "DialogConditions", _log),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "DialogConditions", _log)
                    )
            ).As<IDialogConditionsRepository>().SingleInstance();

            builder.RegisterType<ClientDialogsService>()
                .As<IClientDialogsService>()
                .SingleInstance();
            
            builder.RegisterType<DialogConditionsService>()
                .As<IDialogConditionsService>()
                .SingleInstance();
            
            _services.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_appSettings.CurrentValue.AssetsServiceClient.ServiceUrl),
                _appSettings.CurrentValue.AssetsServiceClient.ExpirationPeriod), _log);
            
            builder.Populate(_services);
        }
    }
}

using System;
using Autofac;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit;
using Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Services;
using Lykke.Service.ClientDialogs.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.ClientDialogs.Modules
{    
    [UsedImplicitly]
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                new ClientDialogsRepository(AzureTableStorage<ClientDialogEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "Dialogs", c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), 
                        "ClientDialogs", c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), 
                        "GlobalDialogs", c.Resolve<ILogFactory>())
                    )
            ).As<IClientDialogsRepository>().SingleInstance();
            
            builder.Register(c =>
                new ClientDialogSubmitsRepository(AzureTableStorage<ClientDialogSubmitEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "SubmittedDialogs",
                    c.Resolve<ILogFactory>()))
            ).As<IClientDialogSubmitsRepository>().SingleInstance();
            
            builder.Register(c =>
                new DialogConditionsRepository(AzureTableStorage<DialogConditionEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "DialogConditions", c.Resolve<ILogFactory>()),
                    AzureTableStorage<AzureIndex>.Create(_appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), 
                        "DialogConditions", c.Resolve<ILogFactory>())
                    )
            ).As<IDialogConditionsRepository>().SingleInstance();

            builder.RegisterType<ClientDialogsService>()
                .As<IClientDialogsService>()
                .SingleInstance();
            
            builder.RegisterType<DialogConditionsService>()
                .As<IDialogConditionsService>()
                .SingleInstance();

            builder.RegisterAssetsClient(AssetServiceSettings.Create(
                new Uri(_appSettings.CurrentValue.AssetsServiceClient.ServiceUrl),
                _appSettings.CurrentValue.AssetsServiceClient.ExpirationPeriod));
        }
    }
}

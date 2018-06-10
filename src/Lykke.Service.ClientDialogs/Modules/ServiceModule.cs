using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog;
using Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Services;
using Lykke.Service.ClientDialogs.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.ClientDialogs.Modules
{    
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<AppSettings> appSettings, ILog log)
        {
            _appSettings = appSettings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(
                new ClientDialogsRepository(AzureTableStorage<ClientDialogEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "Dialogs",
                    _log))
            ).As<IClientDialogsRepository>().SingleInstance();
            
            builder.RegisterInstance(
                new ClientDialogSubmitsRepository(AzureTableStorage<ClientDialogSubmitEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "DialogSubmits",
                   _log))
            ).As<IClientDialogSubmitsRepository>().SingleInstance();

            builder.RegisterType<ClientDialogsService>()
                .As<IClientDialogsService>()
                .SingleInstance();
        }
    }
}

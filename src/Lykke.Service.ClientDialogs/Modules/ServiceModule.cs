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

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
                new ClientDialogsRepository(AzureTableStorage<ClientDialogEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "Dialogs",
                    ctx.Resolve<ILog>()))
            ).As<IClientDialogsRepository>().SingleInstance();
            
            builder.Register(ctx =>
                new ClientDialogSubmitsRepository(AzureTableStorage<ClientDialogSubmitEntity>.Create(
                    _appSettings.ConnectionString(x => x.ClientDialogsService.Db.DataConnString), "DialogSubmits",
                    ctx.Resolve<ILog>()))
            ).As<IClientDialogSubmitsRepository>().SingleInstance();

            builder.RegisterType<ClientDialogsService>()
                .As<IClientDialogsService>()
                .SingleInstance();
        }
    }
}

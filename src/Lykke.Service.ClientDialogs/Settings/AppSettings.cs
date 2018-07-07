using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.ClientDialogs.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public ClientDialogsSettings ClientDialogsService { get; set; }        
        public AssetServiceClientSettings AssetsServiceClient { get; set; }        
    }
}

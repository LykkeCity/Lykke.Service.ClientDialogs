using JetBrains.Annotations;

namespace Lykke.Service.ClientDialogs.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class ClientDialogsSettings
    {
        public DbSettings Db { get; set; }
    }
}

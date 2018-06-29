using System;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ClientDialogs.Settings
{
    public class AssetServiceClientSettings
    {
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
        public TimeSpan ExpirationPeriod { get; set; }
    }
}

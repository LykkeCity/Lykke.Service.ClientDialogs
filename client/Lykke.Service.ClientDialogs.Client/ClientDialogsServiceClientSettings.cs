using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.ClientDialogs.Client 
{
    public class ClientDialogsServiceClientSettings 
    {
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}

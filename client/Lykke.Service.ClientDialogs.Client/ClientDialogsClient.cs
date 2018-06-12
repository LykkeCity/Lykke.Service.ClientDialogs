using Lykke.HttpClientGenerator;

namespace Lykke.Service.ClientDialogs.Client
{
    internal class ClientDialogsClient : IClientDialogsClient
    {
        public IDialogsApi Dialogs { get; }
        public IClientDialogsApi ClientDialogs { get; }

        public ClientDialogsClient(IHttpClientGenerator httpClientGenerator)
        {
            Dialogs = httpClientGenerator.Generate<IDialogsApi>();
            ClientDialogs = httpClientGenerator.Generate<IClientDialogsApi>();
        }
    }
}

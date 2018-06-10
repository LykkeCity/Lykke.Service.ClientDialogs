using JetBrains.Annotations;

namespace Lykke.Service.ClientDialogs.Client
{
    [PublicAPI]
    public interface IClientDialogsClient
    {
        /// <summary>
        /// Api for dialogs management
        /// </summary>
        IDialogsApi Dialogs { get; }
    }
}

using System;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// ClientDialogs validation api exception
    /// </summary>
    [Serializable]
    public class ClientDialogsApiException : Exception
    {
        public ClientDialogsApiException(string message):base(message)
        {
        }
    }
}

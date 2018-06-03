using System;
using System.Net.Http;

namespace Lykke.Service.ClientDialogs.AutorestClient
{
    public partial class ClientDialogsAPI
    {
        /// <inheritdoc />
        /// <summary>
        /// Should be used to prevent memory leak in RetryPolicy
        /// </summary>
        public ClientDialogsAPI(Uri baseUri, HttpClient client) : base(client)
        {
            Initialize();

            BaseUri = baseUri ?? throw new ArgumentNullException("baseUri");
        }
    }
}

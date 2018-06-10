using System;
using System.Net;

namespace Lykke.Service.ClientDialogs.Exceptions
{
    //TODO: move to Lykke.Common.ApiLibrary
    [Serializable]
    public class ValidationApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ValidationApiException(string message) : this(HttpStatusCode.BadRequest, message)
        {
        }
        
        public ValidationApiException(HttpStatusCode code, string message) : base(message)
        {
            StatusCode = code;
        }
    }
}

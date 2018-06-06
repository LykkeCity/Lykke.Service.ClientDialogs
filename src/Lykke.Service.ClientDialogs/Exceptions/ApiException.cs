using System;
using System.Net;

namespace Lykke.Service.ClientDialogs.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(string message) : this(HttpStatusCode.BadRequest, message)
        {
        }
        
        public ApiException(HttpStatusCode code, string message) : base(message)
        {
            StatusCode = code;
        }
    }
}

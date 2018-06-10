using System;
using Lykke.Common.Api.Contract.Responses;

namespace Lykke.Service.ClientDialogs.Client
{
    //TODO: move to Lykke.Common.ApiLibrary
    /// <summary>
    /// client api exception
    /// </summary>
    [Serializable]
    public class ClientApiException : Exception
    {
        public ErrorResponse ErrorResponse { get; set; }
        
        public ClientApiException(ErrorResponse errorResponse):base(errorResponse.ErrorMessage)
        {
            ErrorResponse = errorResponse;
        }
    }
}

using System;
using System.Net;
using System.Threading.Tasks;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.ClientDialogs.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Lykke.Service.ClientDialogs.Middleware
{
    //TODO: move to Lykke.Common.ApiLibrary or add processing to GlobalErrorHandlerMiddleware from Lykke.Common.ApiLibrary
    public class ClientServiceApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientServiceApiExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationApiException exception)
            {
                await CreateErrorResponse(context, exception.Message, exception.StatusCode);
            }
        }
        
        private static async Task CreateErrorResponse(HttpContext ctx, string message, HttpStatusCode status)
        {
            ctx.Response.Clear();
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)status;
            
            var json = JsonConvert.SerializeObject(ErrorResponse.Create(message));

            await ctx.Response.WriteAsync(json);
        }
    }
}

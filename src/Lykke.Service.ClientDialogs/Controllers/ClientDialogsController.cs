using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.ClientDialogs.Client;
using Lykke.Service.ClientDialogs.Client.Models;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ClientDialogs.Controllers
{
    [Route("api/clientdialogs")]
    public class ClientDialogsController : Controller, IClientDialogsApi
    {
        private readonly IClientDialogsService _clientDialogsService;
        private readonly IDialogConditionsService _dialogConditionsService;

        public ClientDialogsController(
            IClientDialogsService clientDialogsService,
            IDialogConditionsService dialogConditionsService)
        {
            _clientDialogsService = clientDialogsService;
            _dialogConditionsService = dialogConditionsService;
        }
        
        /// <summary>
        /// Gets all client dialogs
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}")]
        [SwaggerOperation("GetDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetDialogsAsync(string clientId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            IEnumerable<IClientDialog> dialogs = await _clientDialogsService.GetClientDialogsAsync(clientId);
            var result = Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
            return result;
        }

        /// <summary>
        /// Gets client dialog by Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}/{dialogId}")]
        [SwaggerOperation("GetDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<ClientDialogModel> GetDialogAsync(string clientId, string dialogId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");

            IClientDialog dialog = await _clientDialogsService.GetClientDialogAsync(clientId, dialogId);
            ClientDialogModel result = Mapper.Map<ClientDialogModel>(dialog);
            return result;
        }

        /// <summary>
        /// Assigns the dialog to the specified client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AssignDialogToClient")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AssignDialogToClientAsync([FromBody]AssignDialogRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());
            
            await _clientDialogsService.AssignDialogToClientAsync(request.ClientId, request.DialogId);
        }

        /// <summary>
        /// Deletes client dialog (unassigns dialog from the client)
        /// </summary>
        /// <remarks>This method unassigns dialog from the client</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("")]
        [SwaggerOperation("DeleteDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteDialogAsync([FromBody]DeleteDialogRequest request)
        {
            if (!ModelState.IsValid)
            throw new ValidationApiException(ModelState.GetErrorMessage());
            
            await _clientDialogsService.DeleteDialogAsync(request.ClientId, request.DialogId);
        }
        
        /// <summary>
        /// Gets pretrade client dialogs for specified asset id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}/{assetId}/pretrade")]
        [SwaggerOperation("GetPreTradeDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetPreTradeDialogsAsync(string clientId, string assetId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");

            var dialogs = await _dialogConditionsService.GetDialogsWithPreTradeConditionAsync(clientId, assetId);
            return Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
        }
    }
}

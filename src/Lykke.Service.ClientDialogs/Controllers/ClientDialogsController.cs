using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.ClientDialogs.Client;
using Lykke.Service.ClientDialogs.Client.Models;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
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
        [ProducesResponseType(typeof(IReadOnlyList<DialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetDialogsAsync(string clientId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            IEnumerable<IClientDialog> dialogs = await _clientDialogsService.GetClientDialogsAsync(clientId);
            return Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
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
        [ProducesResponseType(typeof(DialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<ClientDialogModel> GetDialogAsync(string clientId, string dialogId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");

            IClientDialog dialog = await _clientDialogsService.GetClientDialogAsync(clientId, dialogId);
            return Mapper.Map<ClientDialogModel>(dialog);
        }

        /// <summary>
        /// Assigns the dialog to the specified client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AssignDialogToClient")]
        [ProducesResponseType(typeof(DialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AssignDialogToClientAsync([FromBody]AssignDialogRequest request)
        {
            var dialog = await _clientDialogsService.GetDialogAsync(request.DialogId);

            if (dialog == null)
                throw new ValidationApiException("Dialog not found");
            
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
            var dialog = await _clientDialogsService.GetDialogAsync(request.DialogId);
            
            if (dialog == null)
                throw new ValidationApiException("dialog not found");
            
            if (dialog.IsGlobal)
                throw new ValidationApiException("Global dialog can't be deleted");
            
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
        [ProducesResponseType(typeof(IReadOnlyList<DialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetPreTradeDialogsAsync(string clientId, string assetId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");

            var dialogs = await _dialogConditionsService.GetDialogsWithPreTradeConditionAsync(clientId, assetId);
            return Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
        }
        
        /// <summary>
        /// Gets predeposit client dialogs for specified asset id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{clientId}/{assetId}/predeposit")]
        [SwaggerOperation("GetPreDepositDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<DialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetPreDepositDialogsAsync(string clientId, string assetId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");

            var dialogs = await _dialogConditionsService.GetDialogsWithPreDepositConditionAsync(clientId, assetId);
            return Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
        }
    }
}

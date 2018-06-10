using System.Collections.Generic;
using System.Linq;
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
    [Route("api/dialogs")]
    public class DialogsController : Controller, IDialogsApi
    {
        private readonly IClientDialogsService _clientDialogsService;

        public DialogsController(IClientDialogsService clientDialogsService)
        {
            _clientDialogsService = clientDialogsService;
        }

        [HttpGet]
        [Route("{clientId}")]
        [SwaggerOperation("GetClientDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetClientDialogsAsync(string clientId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            var dialogs = await _clientDialogsService.GetDialogsAsync(clientId);
            var result = Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
            return result;
        }
        
        [HttpGet]
        [Route("/api/dialog/{dialogId}")]
        [SwaggerOperation("GetClientDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<ClientDialogModel> GetClientDialogAsync(string dialogId, [FromQuery]string clientId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");
            
            if (!string.IsNullOrEmpty(clientId) && !clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            var dialog = await _clientDialogsService.GetDialogAsync(clientId, dialogId);
            var result = Mapper.Map<ClientDialogModel>(dialog);
            return result;
        }
        
        [HttpGet]
        [Route("common")]
        [SwaggerOperation("GetCommonDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetCommonDialogsAsync()
        {
            var dialogs = await _clientDialogsService.GetCommonDialogsAsync();
            var result = Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
            return result;
        }
        
        [HttpGet]
        [Route("{clientId}/submitted")]
        [SwaggerOperation("GetSubmittedDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<SubmittedDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IReadOnlyList<SubmittedDialogModel>> GetSubmittedDialogsAsync(string clientId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(clientId)} is invalid");
            
            var dialogs = await _clientDialogsService.GetSubmittedDialogsAsync(clientId);
            var result = Mapper.Map<IReadOnlyList<SubmittedDialogModel>>(dialogs);
            return result;
        }
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddClientDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddClientDialogAsync([FromBody] ClientDialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            foreach (var action in model.Actions)
                if (!action.Id.IsValidPartitionOrRowKey())
                    throw new ValidationApiException($"Action Id = {action.Id} is invalid");

            var existingDialog = await _clientDialogsService.GetDialogAsync(model.ClientId, model.Id);

            if (existingDialog != null)
                throw new ValidationApiException($"Dialog with Id = {model.Id} is already exists");

            var dialog = Mapper.Map<IClientDialog>(model);
            await _clientDialogsService.AddDialogAsync(dialog);
        }
        
        [HttpDelete]
        [Route("")]
        [SwaggerOperation("DeleteClientDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteClientDialogAsync([FromBody] DeleteDialogRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            await _clientDialogsService.DeleteDialogAsync(request.ClientId, request.DialogId);
        }
        
        [HttpPost]
        [Route("submit")]
        [SwaggerOperation("SubmitDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task SubmitDialogAsync([FromBody] SubmitDialogRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());
            
            var dialog = await _clientDialogsService.GetDialogAsync(request.ClientId, request.DialogId);
            
            if (dialog == null)
                throw new ValidationApiException("Dialog not found");
            
            if (dialog.Actions.All(item => item.Id != request.ActionId))
                throw new ValidationApiException("Action not found");

            var isSubmitted = await _clientDialogsService.IsDialogSubmittedAsync(request.ClientId, request.DialogId, request.ActionId);
            
            if (isSubmitted)
                throw new ValidationApiException("This dialog is already submitted");
            
            await _clientDialogsService.SubmitDialogAsync(request.ClientId, request.DialogId, request.ActionId);
        }
        
        [HttpPost]
        [Route("isSubmitted")]
        [SwaggerOperation("IsDialogSubmitted")]
        [ProducesResponseType(typeof(SubmittedDialogResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<SubmittedDialogResult> IsDialogSubmittedAsyc([FromBody] SubmitDialogRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());
            
            return new SubmittedDialogResult
            {
                Submitted = await _clientDialogsService.IsDialogSubmittedAsync(request.ClientId, request.DialogId, request.ActionId)
            };
        }
    }
}

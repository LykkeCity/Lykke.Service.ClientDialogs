using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using FluentValidation.AspNetCore;
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
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<ClientDialogModel> AddDialogAsync([FromBody][CustomizeValidator(RuleSet = "new")]ClientDialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            IClientDialog dialog = Mapper.Map<IClientDialog>(model);
            var result = await _clientDialogsService.AddDialogAsync(dialog);
            return Mapper.Map<ClientDialogModel>(result);
        }
        
        [HttpPut]
        [Route("")]
        [SwaggerOperation("UpdateDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<ClientDialogModel> UpdateDialogAsync([FromBody][CustomizeValidator(RuleSet = "*")]ClientDialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            var existingDialog = await _clientDialogsService.GetDialogAsync(model.Id);
            
            if (existingDialog == null)
                throw new ValidationApiException("Dialog not found");
            
            IClientDialog dialog = Mapper.Map<IClientDialog>(model);
            var result = await _clientDialogsService.AddDialogAsync(dialog);
            return Mapper.Map<ClientDialogModel>(result);
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<ClientDialogModel>> GetDialogsAsync()
        {
            IEnumerable<IClientDialog> dialogs = await _clientDialogsService.GetDialogsAsync();
            var result = Mapper.Map<IReadOnlyList<ClientDialogModel>>(dialogs);
            return result;
        }
        
        [HttpGet]
        [Route("{dialogId}")]
        [SwaggerOperation("GetDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<ClientDialogModel> GetDialogAsync(string dialogId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");
            
            IClientDialog dialog = await _clientDialogsService.GetDialogAsync(dialogId);
            ClientDialogModel result = Mapper.Map<ClientDialogModel>(dialog);
            return result;
        }

        [HttpDelete]
        [Route("{dialogId}")]
        [SwaggerOperation("DeleteDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteDialogAsync(string dialogId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");

            await _clientDialogsService.DeleteDialogAsync(dialogId);
        }

        [HttpPost]
        [Route("submit")]
        [SwaggerOperation("SubmitDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task SubmitDialogAsync([FromBody]SubmitDialogRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());
            
            var dialog = await _clientDialogsService.GetDialogAsync(request.DialogId);
            
            if (dialog == null)
                throw new ValidationApiException("Dialog not found");
            
            if (dialog.Actions.All(item => item.Id != request.ActionId))
                throw new ValidationApiException("Action not found");

            var isSubmitted = await _clientDialogsService.IsDialogSubmittedAsync(request.ClientId, request.DialogId, request.ActionId);
            
            if (isSubmitted)
                throw new ValidationApiException("This dialog is already submitted");
            
            await _clientDialogsService.SubmitDialogAsync(request.ClientId, request.DialogId, request.ActionId);
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

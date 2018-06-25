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
        
        /// <summary>
        /// Adds the dialog
        /// </summary>
        /// <remarks>If IsGlobal parameter is set to true, then the dialog becomes a global - and will be assigned to all the clients.</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddDialog")]
        [ProducesResponseType(typeof(DialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<DialogModel> AddDialogAsync([FromBody][CustomizeValidator(RuleSet = "new")]DialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            IClientDialog dialog = Mapper.Map<ClientDialog>(model);
            var result = await _clientDialogsService.AddDialogAsync(dialog);
            return Mapper.Map<DialogModel>(result);
        }
        
        /// <summary>
        /// Updates the dialog
        /// </summary>
        /// <remarks>If IsGlobal parameter is set to true, then the dialog becomes a global - and will be assigned to all the clients.</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        [SwaggerOperation("UpdateDialog")]
        [ProducesResponseType(typeof(DialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<DialogModel> UpdateDialogAsync([FromBody][CustomizeValidator(RuleSet = "*")]DialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            var existingDialog = await _clientDialogsService.GetDialogAsync(model.Id);
            
            if (existingDialog == null)
                throw new ValidationApiException("Dialog not found");
            
            IClientDialog dialog = Mapper.Map<ClientDialog>(model);
            var result = await _clientDialogsService.AddDialogAsync(dialog);
            return Mapper.Map<DialogModel>(result);
        }

        /// <summary>
        /// Gets all dialogs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("GetDialogs")]
        [ProducesResponseType(typeof(IReadOnlyList<DialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<DialogModel>> GetDialogsAsync()
        {
            IEnumerable<IClientDialog> dialogs = await _clientDialogsService.GetDialogsAsync();
            var result = Mapper.Map<IReadOnlyList<DialogModel>>(dialogs);
            return result;
        }
        
        /// <summary>
        /// Gets the dialog by Id
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{dialogId}")]
        [SwaggerOperation("GetDialog")]
        [ProducesResponseType(typeof(DialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<DialogModel> GetDialogAsync(string dialogId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");
            
            IClientDialog dialog = await _clientDialogsService.GetDialogAsync(dialogId);
            DialogModel result = Mapper.Map<DialogModel>(dialog);
            return result;
        }

        /// <summary>
        /// Deletes the dialog
        /// </summary>
        /// <remarks>All the client dialogs assignments and dialog conditions will be deleted aswell</remarks>
        /// <param name="dialogId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Submits the dialog for the specified client and action
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns dialogs submitted by the client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
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

        
        /// <summary>
        /// Checks if the dialog is submitted
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

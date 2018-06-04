using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Extensions;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.ClientDialogs.Controllers
{
    [Route("api/dialogs")]
    public class DialogsController : Controller
    {
        private readonly IClientDialogsService _clientDialogsService;

        public DialogsController(IClientDialogsService clientDialogsService)
        {
            _clientDialogsService = clientDialogsService;
        }

        [HttpGet]
        [Route("{clientId}")]
        [SwaggerOperation("GetClientDialogs")]
        [ProducesResponseType(typeof(IEnumerable<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetClientDialogs(string clientId)
        {
            var dialogs = await _clientDialogsService.GetDialogsAsync(clientId);
            var result = Mapper.Map<IEnumerable<ClientDialogModel>>(dialogs);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("/api/dialog/{dialogId}")]
        [SwaggerOperation("GetClientDialog")]
        [ProducesResponseType(typeof(ClientDialogModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetClientDialog(string dialogId, [FromQuery]string clientId)
        {
            var dialog = await _clientDialogsService.GetDialogAsync(clientId, dialogId);
            var result = Mapper.Map<ClientDialogModel>(dialog);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }
        
        [HttpGet]
        [Route("common")]
        [SwaggerOperation("GetCommonDialogs")]
        [ProducesResponseType(typeof(IEnumerable<ClientDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCommonDialogs()
        {
            var dialogs = await _clientDialogsService.GetCommonDialogsAsync();
            var result = Mapper.Map<IEnumerable<ClientDialogModel>>(dialogs);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{clientId}/submitted")]
        [SwaggerOperation("GetSubmittedDialogs")]
        [ProducesResponseType(typeof(IEnumerable<SubmittedDialogModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSubmittedDialogs(string clientId)
        {
            if (!clientId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(clientId)} is invalid"));
            
            var dialogs = await _clientDialogsService.GetSubmittedDialogsAsync(clientId);
            var result = Mapper.Map<IEnumerable<SubmittedDialogModel>>(dialogs);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddClientDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddClientDialog([FromBody] ClientDialogModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            if (!model.Id.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"Invalid {nameof(model.Id)} value"));
            
            if (model.Actions.Length == 0)
                return BadRequest(ErrorResponse.Create("Specify at least one action"));
            
            foreach (var action in model.Actions)
                if (!action.Id.IsValidPartitionOrRowKey())
                    return BadRequest(ErrorResponse.Create($"Action Id = {action.Id} is invalid"));

            var existingDialog = await _clientDialogsService.GetDialogAsync(model.ClientId, model.Id);

            if (existingDialog != null)
                return BadRequest(ErrorResponse.Create($"Dialog with Id = {model.Id} is already exists"));

            var dialog = Mapper.Map<IClientDialog>(model);
            await _clientDialogsService.AddDialogAsync(dialog);
            return Ok();
        }
        
        [HttpDelete]
        [Route("")]
        [SwaggerOperation("DeleteClientDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteClientDialog([FromBody] DeleteDialogRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));

            await _clientDialogsService.DeleteDialogAsync(request.ClientId, request.DialogId);
            return Ok();
        }
        
        [HttpPost]
        [Route("submit")]
        [SwaggerOperation("SubmitDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SubmitDialog([FromBody] SubmitDialogRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));
            
            if (!request.ClientId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.ClientId)} is invalid"));
            
            if (!request.DialogId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.DialogId)} is invalid"));
            
            if (!request.ActionId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.ActionId)} is invalid"));

            var isSubmitted = await _clientDialogsService.IsDialogSubmittedAsync(request.ClientId, request.DialogId, request.ActionId);

            var dialog = await _clientDialogsService.GetDialogAsync(request.ClientId, request.DialogId);
            
            if (dialog == null)
                return BadRequest(ErrorResponse.Create("Dialog not found"));
            
            if (dialog.Actions.All(item => item.Id != request.ActionId))
                return BadRequest(ErrorResponse.Create("Action not found"));
            
            if (isSubmitted)
                return BadRequest(ErrorResponse.Create("This dialog is already submitted"));
            
            await _clientDialogsService.SubmitDialogAsync(request.ClientId, request.DialogId, request.ActionId);
            return Ok();
        }
        
        [HttpPost]
        [Route("isSubmitted")]
        [SwaggerOperation("IsDialogSubmitted")]
        [ProducesResponseType(typeof(SubmittedDialogResult), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task<IActionResult> IsDialogSubmitted([FromBody] SubmitDialogRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ErrorResponse.Create(ModelState.GetErrorMessage()));
            
            if (!request.ClientId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.ClientId)} is invalid"));
            
            if (!request.DialogId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.DialogId)} is invalid"));
            
            if (!request.ActionId.IsValidPartitionOrRowKey())
                return BadRequest(ErrorResponse.Create($"{nameof(request.ActionId)} is invalid"));

            var result = new SubmittedDialogResult
            {
                Submitted = await _clientDialogsService.IsDialogSubmittedAsync(request.ClientId, request.DialogId, request.ActionId)
            };
            
            return Ok(result);
        }
    }
}

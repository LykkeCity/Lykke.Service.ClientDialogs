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
using Lykke.Service.ClientDialogs.Core.Domain.ConditionParameters;
using Lykke.Service.ClientDialogs.Core.Services;
using Lykke.Service.ClientDialogs.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using DialogConditionType = Lykke.Service.ClientDialogs.Client.Models.DialogConditionType;

namespace Lykke.Service.ClientDialogs.Controllers
{
    [Route("api/conditions")]
    public class DialogConditionsController : Controller, IDialogConditionsApi
    {
        private readonly IDialogConditionsService _dialogConditionsService;

        public DialogConditionsController(IDialogConditionsService dialogConditionsService)
        {
            _dialogConditionsService = dialogConditionsService;
        }

        [HttpPost]
        [Route("pretrade")]
        [SwaggerOperation("AddPreTradeDialogCondition")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddPreTradeDialogConditionDAsync([FromBody]PreTradeConditionRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());
            
            if (!string.IsNullOrEmpty(request.Id) && !request.Id.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(request.Id)} is invalid");

            await _dialogConditionsService.AddDialogConditionAsync(new DialogCondition
            {
                Id = request.Id,
                DialogId = request.DialogId,
                Type = Core.Domain.DialogConditionType.Pretrade,
                Data = new PreTradeParameters
                {
                    AssetId = request.AssetId
                }.ToJson()
            });
        }
        
        [HttpDelete]
        [Route("")]
        [SwaggerOperation("DeleteDialogCondition")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteDialogConditionDAsync([FromBody]DeleteDialogConditionRequest request)
        {
            if (!ModelState.IsValid)
                throw new ValidationApiException(ModelState.GetErrorMessage());

            await _dialogConditionsService.DeleteDialogConditionAsync(request.DialogId, request.ConditionId, 
                Mapper.Map<Core.Domain.DialogConditionType>(request.Type));
        }

        [HttpDelete]
        [Route("{dialogId}")]
        [SwaggerOperation("DeleteDialogConditions")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteDialogConditionsAsync(string dialogId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");

            await _dialogConditionsService.DeleteDialogConditionsAsync(dialogId);
        }

        [HttpGet]
        [Route("{type}/type")]
        [SwaggerOperation("GetConditionsByType")]
        [ProducesResponseType(typeof(IReadOnlyList<DialogConditionModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<DialogConditionModel>> GetDialogConditionsByTypeAsync(DialogConditionType type)
        {
            var conditions = await _dialogConditionsService.GetDialogConditionsByTypeAsync(Mapper.Map<Core.Domain.DialogConditionType>(type));

            return Mapper.Map<IReadOnlyList<DialogConditionModel>>(conditions);
        }
        
        [HttpGet]
        [Route("{dialogId}/dialog")]
        [SwaggerOperation("GetDialogConditions")]
        [ProducesResponseType(typeof(IReadOnlyList<DialogConditionModel>), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<IReadOnlyList<DialogConditionModel>> GetDialogConditionsAsync(string dialogId)
        {
            var conditions = await _dialogConditionsService.GetDialogConditionsAsync(dialogId);

            return Mapper.Map<IReadOnlyList<DialogConditionModel>>(conditions);
        }
    }
}

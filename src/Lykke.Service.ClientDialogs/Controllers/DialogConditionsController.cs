using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Service.Assets.Client;
using Lykke.Service.ClientDialogs.Client;
using Lykke.Service.ClientDialogs.Client.Models;
using Lykke.Service.ClientDialogs.Core.Domain;
using Lykke.Service.ClientDialogs.Core.Domain.ConditionParameters;
using Lykke.Service.ClientDialogs.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using DialogConditionType = Lykke.Service.ClientDialogs.Core.Domain.DialogConditionType;

namespace Lykke.Service.ClientDialogs.Controllers
{
    [Route("api/conditions")]
    public class DialogConditionsController : Controller, IDialogConditionsApi
    {
        private readonly IDialogConditionsService _dialogConditionsService;
        private readonly IAssetsServiceWithCache _assetsService;

        public DialogConditionsController(
            IDialogConditionsService dialogConditionsService,
            IAssetsServiceWithCache assetsService)
        {
            _dialogConditionsService = dialogConditionsService;
            _assetsService = assetsService;
        }

        /// <summary>
        /// Adds new pretrade dialog condition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pretrade")]
        [SwaggerOperation("AddPreTradeDialogCondition")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddPreTradeDialogConditionAsync([FromBody]PreTradeConditionRequest request)
        {
            var condition = await _dialogConditionsService.GetDialogConditionAsync(request.DialogId);
            
            if (condition != null && condition.Type != DialogConditionType.Pretrade)
                throw new ValidationApiException($"Dialog already has a {condition.Type} condition");

            var asset = await _assetsService.TryGetAssetAsync(request.AssetId);
            
            if (asset == null)
                throw new ValidationApiException($"Asset {request.AssetId} not found");
            
            await _dialogConditionsService.AddDialogConditionAsync(new DialogCondition
            {
                DialogId = request.DialogId,
                Type = DialogConditionType.Pretrade,
                Data = new PreTradeParameters
                {
                    AssetId = request.AssetId
                }.ToJson()
            });
        }

        /// <summary>
        /// Adds new predeposit dialog condition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("predeposit")]
        [SwaggerOperation("AddPreDepositDialogCondition")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddPreDepositDialogConditionAsync([FromBody]PreDepositConditionRequest request)
        {
            var condition = await _dialogConditionsService.GetDialogConditionAsync(request.DialogId);
            
            if (condition != null && condition.Type != DialogConditionType.Predeposit)
                throw new ValidationApiException($"Dialog already has a {condition.Type} condition");

            foreach (var assetId in request.AssetIds)
            {
                var asset = await _assetsService.TryGetAssetAsync(assetId);
            
                if (asset == null)
                    throw new ValidationApiException($"Asset {assetId} not found");
            }
            
            await _dialogConditionsService.AddDialogConditionAsync(new DialogCondition
            {
                DialogId = request.DialogId,
                Type = DialogConditionType.Predeposit,
                Data = new PreDepositParameters
                {
                    AssetIds = request.AssetIds
                }.ToJson()
            });
        }

        /// <summary>
        /// Deletes dialog condition
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{dialogId}")]
        [SwaggerOperation("DeleteDialogCondition")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task DeleteDialogConditionAsync(string dialogId)
        {
            if (!dialogId.IsValidPartitionOrRowKey())
                throw new ValidationApiException($"{nameof(dialogId)} is invalid");

            await _dialogConditionsService.DeleteDialogConditionAsync(dialogId);
        }

        /// <summary>
        /// Gets dialog condition
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{dialogId}")]
        [SwaggerOperation("GetDialogCondition")]
        [ProducesResponseType(typeof(DialogConditionModel), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.InternalServerError)]
        public async Task<DialogConditionModel> GetDialogConditionAsync(string dialogId)
        {
            var condition = await _dialogConditionsService.GetDialogConditionAsync(dialogId);

            return Mapper.Map<DialogConditionModel>(condition);
        }
    }
}

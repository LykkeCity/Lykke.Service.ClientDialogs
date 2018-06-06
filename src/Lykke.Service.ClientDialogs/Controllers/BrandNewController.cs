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
    [Route("api/new")]
    public class BrandNewController : Controller, IBrandNewApi
    {
        private readonly IClientDialogsService _clientDialogsService;

        public BrandNewController(IClientDialogsService clientDialogsService)
        {
            _clientDialogsService = clientDialogsService;
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
        
        [HttpPost]
        [Route("")]
        [SwaggerOperation("AddClientDialog")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.BadRequest)]
        public async Task AddClientDialogAsync(ClientDialogModel model)
        {
            if (!ModelState.IsValid)
                throw new ApiException(ModelState.GetErrorMessage());

            if (!model.Id.IsValidPartitionOrRowKey())
                throw new ApiException($"Invalid {nameof(model.Id)} value");
            
            if (model.Actions.Length == 0)
                throw new ApiException("Specify at least one action");
            
            foreach (var action in model.Actions)
                if (!action.Id.IsValidPartitionOrRowKey())
                    throw new ApiException($"Action Id = {action.Id} is invalid");

            var existingDialog = await _clientDialogsService.GetDialogAsync(model.ClientId, model.Id);

            if (existingDialog != null)
                throw new ApiException($"Dialog with Id = {model.Id} is already exists");

            var dialog = Mapper.Map<IClientDialog>(model);
            await _clientDialogsService.AddDialogAsync(dialog);
        }
    }
}

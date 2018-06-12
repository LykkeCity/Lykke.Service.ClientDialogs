﻿using System.Collections.Generic;
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

        public ClientDialogsController(IClientDialogsService clientDialogsService)
        {
            _clientDialogsService = clientDialogsService;
        }
        
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
    }
}

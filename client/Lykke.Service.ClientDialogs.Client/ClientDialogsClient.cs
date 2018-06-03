using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.AutorestClient;
using Lykke.Service.ClientDialogs.AutorestClient.Models;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <inheritdoc cref="IClientDialogsClient"/>>
    public class ClientDialogsClient : IClientDialogsClient, IDisposable
    {
        private ClientDialogsAPI _service;
        private bool _disposed;

        /// <inheritdoc />>
        public ClientDialogsClient(string serviceUrl)
        {
            _service = new ClientDialogsAPI(new Uri(serviceUrl), new HttpClient());
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || _disposed)
                return;
            
            if (_service == null)
                return;
            
            _service.Dispose();
            _service = null;
            _disposed = true;
        }

        /// <inheritdoc />>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />>
        public async Task AddDialogAsync(ClientDialogModel clientDialog)
        {
            var response = await _service.AddClientDialogAsync(clientDialog);

            if (response != null)
                throw new ClientDialogsApiException(response.ErrorMessage);
        }

        /// <inheritdoc />>
        public async Task<ClientDialogModel> GetDialogAsync(string clientId, string dialogId)
        {
            var response = await _service.GetClientDialogAsync(dialogId, clientId);
            
            switch (response)
            {
                case ClientDialogModel result:
                    return result;
                case ErrorResponse error:
                    throw new ClientDialogsApiException(error.ErrorMessage);
            }

            return null;
        }

        /// <inheritdoc />>
        public async Task<IEnumerable<ClientDialogModel>> GetDialogsAsync(string clientId)
        {
            var response = await _service.GetClientDialogsAsync(clientId);
            
            switch (response)
            {
                case List<ClientDialogModel> result:
                    return result;
                case ErrorResponse error:
                    throw new ClientDialogsApiException(error.ErrorMessage);
            }

            return Array.Empty<ClientDialogModel>();
        }

        /// <inheritdoc />>
        public async Task<IEnumerable<ClientDialogModel>> GetCommonDialogsAsync()
        {
            var response = await _service.GetCommonDialogsAsync();
            
            switch (response)
            {
                case List<ClientDialogModel> result:
                    return result;
                case ErrorResponse error:
                    throw new ClientDialogsApiException(error.ErrorMessage);
            }

            return Array.Empty<ClientDialogModel>();
        }

        /// <inheritdoc />>
        public async Task DeleteDialogAsync(string clientId, string dialogId)
        {
            var response = await _service.DeleteClientDialogAsync(new DeleteDialogRequest(clientId, dialogId));
            
            if (response != null)
                throw new ClientDialogsApiException(response.ErrorMessage);
        }

        /// <inheritdoc />>
        public async Task SubmitDialogAsync(string clientId, string dialogId, string actionId)
        {
            var response = await _service.SubmitDialogAsync(new SubmitDialogRequest(clientId, dialogId, actionId));
            
            if (response != null)
                throw new ClientDialogsApiException(response.ErrorMessage);
        }
        
        /// <inheritdoc />>
        public async Task<IEnumerable<SubmittedDialogModel>> GetSubmittedDialogsAsync(string clientId)
        {
            var response = await _service.GetSubmittedDialogsAsync(clientId);
            
            switch (response)
            {
                case List<SubmittedDialogModel> result:
                    return result;
                case ErrorResponse error:
                    throw new ClientDialogsApiException(error.ErrorMessage);
            }

            return Array.Empty<SubmittedDialogModel>();
        }

        /// <inheritdoc />>
        public async Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId)
        {
            var response = await _service.IsDialogSubmittedAsync(new SubmitDialogRequest(clientId, dialogId, actionId));
            
            switch (response)
            {
                case bool result:
                    return result;
                case ErrorResponse error:
                    throw new ClientDialogsApiException(error.ErrorMessage);
            }

            return false;
        }
    }
}

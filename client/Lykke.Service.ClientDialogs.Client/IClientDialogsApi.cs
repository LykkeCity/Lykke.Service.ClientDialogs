using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Client.Models;
using Refit;

namespace Lykke.Service.ClientDialogs.Client
{
    public interface IClientDialogsApi
    {
        /// <summary>
        /// Gets client dialogs including common dialogs
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/clientdialogs/{clientId}")]
        Task<IReadOnlyList<ClientDialogModel>> GetDialogsAsync(string clientId);
        
        /// <summary>
        /// Gets client dialog
        /// </summary>
        /// <param name="dialogId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/clientdialogs/{clientId}/{dialogId}")]
        Task<ClientDialogModel> GetDialogAsync(string clientId, string dialogId);
        
        /// <summary>
        /// Assigns dialog to the client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/clientdialogs")]
        Task AssignDialogToClientAsync([Body]AssignDialogRequest request);
        
        /// <summary>
        /// Deletes client dialog
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Delete("/api/clientdialogs")]
        Task DeleteDialogAsync([Body]DeleteDialogRequest request);
        
        /// <summary>
        /// Gets pretrade client dialogs for asset
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [Get("/api/clientdialogs/{clientId}/{assetId}/pretrade")]
        Task<IReadOnlyList<ClientDialogModel>> GetPreTradeDialogsAsync(string clientId, string assetId);
    }
}

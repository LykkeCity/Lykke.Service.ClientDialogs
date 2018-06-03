using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.AutorestClient.Models;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// Client for dialogs management (client or common dialogs)
    /// </summary>
    public interface IClientDialogsClient
    {
        /// <summary>
        /// Adds client dialog or common dialog (if clientId = null)
        /// </summary>
        /// <param name="clientDialog"></param>
        /// <returns></returns>
        Task AddDialogAsync(ClientDialogModel clientDialog);
        
        /// <summary>
        /// Gets dialog by id
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        Task<ClientDialogModel> GetDialogAsync(string clientId, string dialogId);
        
        /// <summary>
        /// Gets all client dialogs (including common dialogs)
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<ClientDialogModel>> GetDialogsAsync(string clientId);
        
        /// <summary>
        /// Gets all common dialogs
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ClientDialogModel>> GetCommonDialogsAsync();
        
        /// <summary>
        /// Deletes dialog
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        Task DeleteDialogAsync(string clientId, string dialogId);
        
        /// <summary>
        /// Submits client dialog
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dialogId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        Task SubmitDialogAsync(string clientId, string dialogId, string actionId);
        
        /// <summary>
        /// Gets information about submitted client dialogs
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<IEnumerable<SubmittedDialogModel>> GetSubmittedDialogsAsync(string clientId);
        
        /// <summary>
        /// Checks if the cliend dialog already submitted
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="dialogId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId);
    }
}

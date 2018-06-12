using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Client.Models;
using Refit;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// Dialogs service
    /// </summary>
    public interface IDialogsApi
    {
        /// <summary>
        /// Adds new dialog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Post("/api/dialogs")]
        Task<ClientDialogModel> AddDialogAsync([Body]ClientDialogModel model);
        
        /// <summary>
        /// Updates new dialog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Put("/api/dialogs")]
        Task<ClientDialogModel> UpdateDialogAsync([Body]ClientDialogModel model);
                
        /// <summary>
        /// Gets all dialogs
        /// </summary>
        /// <returns></returns>
        [Get("/api/dialogs")]
        Task<IReadOnlyList<ClientDialogModel>> GetDialogsAsync();
        
        /// <summary>
        /// Gets dialog by id
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [Get("/api/dialogs/{dialogId}")]
        Task<ClientDialogModel> GetDialogAsync(string dialogId);
        
        /// <summary>
        /// Deletes dialog and assignments (client and common)
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [Delete("/api/dialogs/{dialogId}")]
        Task DeleteDialogAsync(string dialogId);
        
        /// <summary>
        /// Sumbits client dialog
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/dialogs/submit")]
        Task SubmitDialogAsync([Body]SubmitDialogRequest request);
        
        /// <summary>
        /// Gets dialogs submitted by client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/dialogs/{clientId}/submitted")]
        Task<IReadOnlyList<SubmittedDialogModel>> GetSubmittedDialogsAsync(string clientId);
        
        /// <summary>
        /// Checks if the dialog is already submitted by the client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/dialogs/isSubmitted")]
        Task<SubmittedDialogResult> IsDialogSubmittedAsyc([Body] SubmitDialogRequest request);
    }
}

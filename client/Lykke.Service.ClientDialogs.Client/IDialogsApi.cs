using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Client.Models;
using Refit;

namespace Lykke.Service.ClientDialogs.Client
{
    /// <summary>
    /// Client dialogs service
    /// </summary>
    public interface IDialogsApi
    {
        /// <summary>
        /// Gets client dialogs including common dialogs
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/dialogs/{clientId}")]
        Task<IReadOnlyList<ClientDialogModel>> GetClientDialogsAsync(string clientId);
        
        /// <summary>
        /// Gets client dialog
        /// </summary>
        /// <param name="dialogId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/dialog/{dialogId}")]
        Task<ClientDialogModel> GetClientDialogAsync(string dialogId, string clientId);
        
        /// <summary>
        /// Gets common dialogs
        /// </summary>
        /// <returns></returns>
        [Get("/api/dialogs/common")]
        Task<IReadOnlyList<ClientDialogModel>> GetCommonDialogsAsync();
        
        /// <summary>
        /// Gets dialogs submitted by client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [Get("/api/dialogs/{clientId}/submitted")]
        Task<IReadOnlyList<SubmittedDialogModel>> GetSubmittedDialogsAsync(string clientId);
        
        /// <summary>
        /// Adds client dialog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Post("/api/dialogs")]
        Task AddClientDialogAsync([Body]ClientDialogModel model);
        
        /// <summary>
        /// Deletes client dialog
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Delete("/api/dialogs")]
        Task DeleteClientDialogAsync([Body]DeleteDialogRequest request);
        
        /// <summary>
        /// Sumbits client dialog
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/dialogs/submit")]
        Task SubmitDialogAsync([Body]SubmitDialogRequest request);
        
        /// <summary>
        /// Checks if the dialog is already submitted by the client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/dialogs/isSubmitted")]
        Task<SubmittedDialogResult> IsDialogSubmittedAsyc([Body] SubmitDialogRequest request);
    }
}

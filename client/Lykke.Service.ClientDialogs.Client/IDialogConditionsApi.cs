using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Client.Models;
using Refit;

namespace Lykke.Service.ClientDialogs.Client
{
    public interface IDialogConditionsApi
    {
        /// <summary>
        /// Adds a pretrade dialog condition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/conditions/pretrade")]
        Task AddPreTradeDialogConditionDAsync([Body] PreTradeConditionRequest request);

        /// <summary>
        /// Deletes dialog condition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Delete("/api/conditions")]
        Task DeleteDialogConditionDAsync([Body] DeleteDialogConditionRequest request);
        
        /// <summary>
        /// Deletes dialog conditions
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [Delete("/api/conditions/{dialogId}")]
        Task DeleteDialogConditionsAsync(string dialogId);

        /// <summary>
        /// Gets all conditions by type
        /// </summary>
        /// <returns></returns>
        [Get("/api/conditions/{type}/type")]
        Task<IReadOnlyList<DialogConditionModel>> GetDialogConditionsByTypeAsync(DialogConditionType type);
        
        /// <summary>
        /// Gets all dialog conditions
        /// </summary>
        /// <returns></returns>
        [Get("/api/conditions/{dialogId}/dialog")]
        Task<IReadOnlyList<DialogConditionModel>> GetDialogConditionsAsync(string dialogId);
    }
}

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
        Task AddPreTradeDialogConditionAsync([Body] PreTradeConditionRequest request);

        /// <summary>
        /// Deletes dialog condition
        /// </summary>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        [Delete("/api/conditions/{dialogId}")]
        Task DeleteDialogConditionAsync(string dialogId);
        
        /// <summary>
        /// Gets dialog condition
        /// </summary>
        /// <returns></returns>
        [Get("/api/conditions/{dialogId}")]
        Task<DialogConditionModel> GetDialogConditionAsync(string dialogId);
    }
}

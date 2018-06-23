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
        /// Adds a predeposit dialog condition
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Post("/api/conditions/predeposit")]
        Task AddPreDepositDialogConditionDAsync([Body] PreDepositConditionRequest request);
        
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

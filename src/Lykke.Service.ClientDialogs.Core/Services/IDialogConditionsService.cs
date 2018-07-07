using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Core.Services
{
    public interface IDialogConditionsService
    {
        Task<IEnumerable<IClientDialog>> GetDialogsWithPreTradeConditionAsync(string clientId, string assetId);
        Task<IEnumerable<IClientDialog>> GetDialogsWithPreDepositConditionAsync(string clientId, string assetId);
        Task<IDialogCondition> GetDialogConditionAsync(string dialogId);
        Task AddDialogConditionAsync(IDialogCondition condition);
        Task DeleteDialogConditionAsync(string dialogId);
    }
}

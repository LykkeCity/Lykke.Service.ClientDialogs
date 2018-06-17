using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Core.Services
{
    public interface IDialogConditionsService
    {
        Task<IEnumerable<IClientDialog>> GetDialogsWithPreTradeConditionAsync(string clientId, string assetId);
        Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(string dialogId);
        Task<IEnumerable<IDialogCondition>> GetDialogConditionsByTypeAsync(DialogConditionType type);
        Task<IDialogCondition> GetDialogConditionAsync(string dialogId, string id);
        Task AddDialogConditionAsync(IDialogCondition condition);
        Task DeleteDialogConditionAsync(string dialogId, string id, DialogConditionType type);
        Task DeleteDialogConditionsAsync(string dialogId);
    }
}

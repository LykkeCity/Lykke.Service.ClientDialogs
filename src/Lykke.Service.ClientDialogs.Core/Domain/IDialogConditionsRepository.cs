using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IDialogConditionsRepository
    {
        Task AddDialogConditionAsync(IDialogCondition condition);
        Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(string dialogId);
        Task<IEnumerable<IDialogCondition>> GetDialogConditionsByTypeAsync(DialogConditionType type);
        Task<IDialogCondition> GetDialogConditionAsync(string dialogId, string id);
        Task DeleteDialogConditionAsync(string dialogId, string id, DialogConditionType type);
        Task DeleteDialogConditionsAsync(string dialogId);
    }
}

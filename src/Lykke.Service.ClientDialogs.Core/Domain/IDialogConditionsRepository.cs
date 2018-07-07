using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IDialogConditionsRepository
    {
        Task AddDialogConditionAsync(IDialogCondition condition);
        Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(DialogConditionType type);
        Task<IDialogCondition> GetDialogConditionAsync(string dialogId);
        Task DeleteDialogConditionAsync(string dialogId);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialogsRepository
    {
        Task<IClientDialog> AddDialogAsync(IClientDialog clientDialog);
        Task<IEnumerable<IClientDialog>> GetDialogsAsync();
        Task<IClientDialog> GetDialogAsync(string dialogId);
        Task AssignDialogToAllAsync(string dialogId);
        Task UnAssignCommonDialogAsync(string dialogId);
        Task DeleteDialogAsync(string dialogId);
        
        Task<IEnumerable<IClientDialog>> GetClientDialogsAsync(string clientId);
        Task<IClientDialog> GetClientDialogAsync(string clientId, string dialogId);
        Task AssignDialogToClientAsync(string clientId, string dialogId);
        Task DeleteDialogAsync(string clientId, string dialogId);
        
        Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Core.Services
{
    public interface IClientDialogsService
    {
        Task<IClientDialog> AddDialogAsync(IClientDialog clientDialog);
        Task<IEnumerable<IClientDialog>> GetDialogsAsync();
        Task<IClientDialog> GetDialogAsync(string dialogId);
        Task AssignDialogToAllAsync(string dialogId);
        Task UnAssignGlobalDialogAsync(string dialogId);
        Task DeleteDialogAsync(string dialogId);
        
        Task<IEnumerable<IClientDialog>> GetClientDialogsAsync(string clientId);
        Task<IClientDialog> GetClientDialogAsync(string clientId, string dialogId);
        Task AssignDialogToClientAsync(string clientId, string dialogId);
        Task DeleteDialogAsync(string clientId, string dialogId);
        
        Task SubmitDialogAsync(string clientId, string dialogId, string actionId);
        Task<IEnumerable<IClientDialogSubmit>> GetSubmittedDialogsAsync(string clientId);
        Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId);

        Task<IEnumerable<IClientDialog>> GetGlobalDialogsAsync();
    }
}

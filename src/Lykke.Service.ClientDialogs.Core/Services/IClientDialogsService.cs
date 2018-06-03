using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Core.Services
{
    public interface IClientDialogsService
    {
        Task AddDialogAsync(IClientDialog clientDialog);
        Task<IClientDialog> GetDialogAsync(string clientId, string dialogId);
        Task<IEnumerable<IClientDialog>> GetDialogsAsync(string clientId);
        Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync();
        Task DeleteDialogAsync(string clientId, string dialogId);
        
        Task SubmitDialogAsync(string clientId, string dialogId, string actionId);
        Task<IEnumerable<IClientDialogSubmit>> GetSubmittedDialogsAsync(string clientId);
        Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId);
    }
}

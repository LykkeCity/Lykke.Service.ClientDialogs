using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialogsRepository
    {
        Task AddDialogAsync(IClientDialog clientDialog);
        Task<IClientDialog> GetDialogAsync(string clientId, string dialogId);
        Task<IEnumerable<IClientDialog>> GetDialogsAsync(string clientId);
        Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync();
        Task DeleteDialogAsync(string clientId, string dialogId);
    }
}

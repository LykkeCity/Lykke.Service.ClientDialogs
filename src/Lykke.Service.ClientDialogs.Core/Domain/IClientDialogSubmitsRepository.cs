using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialogSubmitsRepository
    {
        Task SubmitDialogAsync(string clientId, string dialogId, string actionId);
        Task<IEnumerable<IClientDialogSubmit>> GetSubmittedDialogsAsync(string clientId);
        Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId);
    }
}

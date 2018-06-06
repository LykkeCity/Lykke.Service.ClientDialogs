using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.ClientDialogs.Client.Models;
using Refit;

namespace Lykke.Service.ClientDialogs.Client
{
    public interface IBrandNewApi
    {
        [Get("/api/new/common")]
        Task<IReadOnlyList<ClientDialogModel>> GetCommonDialogsAsync();
        [Post("/api/new")]
        Task AddClientDialogAsync(ClientDialogModel model);
    }
}

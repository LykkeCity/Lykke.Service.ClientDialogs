using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit
{
    public class ClientDialogSubmitsRepository : IClientDialogSubmitsRepository
    {
        private readonly INoSQLTableStorage<ClientDialogSubmitEntity> _tableStorage;

        public ClientDialogSubmitsRepository(INoSQLTableStorage<ClientDialogSubmitEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task SubmitDialogAsync(string clientId, string dialogId, string actionId)
        {
            await _tableStorage.InsertOrReplaceAsync(ClientDialogSubmitEntity.Create(clientId, dialogId, actionId));
        }

        public async Task<IEnumerable<IClientDialogSubmit>> GetSubmittedDialogsAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(ClientDialogSubmitEntity.GeneratePartitionKey(clientId));
        }

        public Task<bool> IsDialogSubmittedAsync(string clientId, string dialogId, string actionId)
        {
            return _tableStorage.RecordExistsAsync(ClientDialogSubmitEntity.Create(clientId, dialogId, actionId));
        }
    }
}

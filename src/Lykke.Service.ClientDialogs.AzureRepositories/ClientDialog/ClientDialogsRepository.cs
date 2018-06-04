using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog
{
    public class ClientDialogsRepository : IClientDialogsRepository
    {
        private readonly INoSQLTableStorage<ClientDialogEntity> _tableStorage;

        public ClientDialogsRepository(INoSQLTableStorage<ClientDialogEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task AddDialogAsync(IClientDialog clientDialog)
        {
            var entity = ClientDialogEntity.Create(clientDialog);
            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task<IClientDialog> GetDialogAsync(string clientId, string dialogId)
        {
            return await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(clientId),
                ClientDialogEntity.GenerateRowKey(dialogId))
                ?? await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(null),
                       ClientDialogEntity.GenerateRowKey(dialogId));
        }

        public async Task<IEnumerable<IClientDialog>> GetDialogsAsync(string clientId)
        {
            var dialogs = (await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(clientId))).ToList();
            
            var commonDialogs = (await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(null))).ToList();

            dialogs.AddRange(commonDialogs);

            return dialogs;
        }

        public async Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync()
        {
            return await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(null));
        }

        public Task DeleteDialogAsync(string clientId, string dialogId)
        {
            return _tableStorage.DeleteIfExistAsync(ClientDialogEntity.GeneratePartitionKey(clientId), ClientDialogEntity.GenerateRowKey(dialogId));
        }
    }
}

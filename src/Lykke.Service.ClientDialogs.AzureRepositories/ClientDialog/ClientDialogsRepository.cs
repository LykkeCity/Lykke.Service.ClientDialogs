using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog
{
    public class ClientDialogsRepository : IClientDialogsRepository
    {
        private readonly INoSQLTableStorage<ClientDialogEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _clientDialogIndex;
        private readonly INoSQLTableStorage<AzureIndex> _commonDialogIndex;

        public ClientDialogsRepository(INoSQLTableStorage<ClientDialogEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> clientDialogIndex,
            INoSQLTableStorage<AzureIndex> commonDialogIndex)
        {
            _tableStorage = tableStorage;
            _clientDialogIndex = clientDialogIndex;
            _commonDialogIndex = commonDialogIndex;
        }

        public async Task<IClientDialog> AddDialogAsync(IClientDialog clientDialog)
        {
            var entity = ClientDialogEntity.Create(clientDialog);
            await _tableStorage.InsertOrMergeAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<IClientDialog>> GetDialogsAsync()
        {
            return await _tableStorage.GetDataAsync();
        }

        public async Task<IClientDialog> GetDialogAsync(string dialogId)
        {
            return await _tableStorage.GetDataAsync(ClientDialogEntity.GeneratePartitionKey(),
                ClientDialogEntity.GenerateRowKey(dialogId));
        }

        public Task AssignDialogToAllAsync(string dialogId)
        {
            var indexEntity = CreateCommonDialogIndex(dialogId);
            return _commonDialogIndex.InsertOrMergeAsync(indexEntity);
        }

        public Task UnAssignCommonDialogAsync(string dialogId)
        {
            return _commonDialogIndex.DeleteIfExistAsync(ClientDialogEntity.GenerateCommonDialogPartitionKey(), dialogId);
        }

        public async Task DeleteDialogAsync(string dialogId)
        {
            await _tableStorage.DeleteIfExistAsync(ClientDialogEntity.GeneratePartitionKey(), ClientDialogEntity.GenerateRowKey(dialogId));

            var tasks = new List<Task>();
            
            var dialogIndexes = (await _clientDialogIndex.GetDataAsync(ClientDialogEntity.GenerateDialogIndex(dialogId))).ToList();

            foreach (var index in dialogIndexes)
            {
                tasks.Add(_clientDialogIndex.DeleteIfExistAsync(index.RowKey, dialogId));
                tasks.Add(_clientDialogIndex.DeleteIfExistAsync(ClientDialogEntity.GenerateDialogIndex(dialogId), index.RowKey));
            }
            
            tasks.Add(UnAssignCommonDialogAsync(dialogId));

            await Task.WhenAny(tasks);
        }

        public Task SetDialogConditionTypeAsync(string dialogId, DialogConditionType? type)
        {
            return _tableStorage.ReplaceAsync(ClientDialogEntity.GeneratePartitionKey(),
                ClientDialogEntity.GenerateRowKey(dialogId),
                entity =>
                {
                    entity.ConditionType = type;
                    return entity;
                });
        }

        public async Task<IEnumerable<IClientDialog>> GetClientDialogsAsync(string clientId)
        {
            var indexes = (await _clientDialogIndex.GetDataAsync(clientId)).ToList();
            var clientDialogs = (await _tableStorage.GetDataAsync(indexes)).ToList();
            
            var commonIndexes = (await _commonDialogIndex.GetDataAsync(ClientDialogEntity.GenerateCommonDialogPartitionKey())).ToList();
            var commonDialogs = (await _tableStorage.GetDataAsync(commonIndexes)).ToList();

            clientDialogs.AddRange(commonDialogs.Where(item=> clientDialogs.All(x => x.Id != item.Id)));

            return clientDialogs;
        }

        public async Task<IClientDialog> GetClientDialogAsync(string clientId, string dialogId)
        {
            var index = await _clientDialogIndex.GetDataAsync(clientId, dialogId);
            
            return index == null 
                ? null
                : await _tableStorage.GetDataAsync(index);
        }

        public async Task AssignDialogToClientAsync(string clientId, string dialogId)
        {
            var index = CreateClientDialogIndex(clientId, dialogId);
            var dialogIndex = CreateDialogIndex(clientId, dialogId);

            var tasks = new List<Task>
            {
                _clientDialogIndex.InsertOrMergeAsync(index),
                _clientDialogIndex.InsertOrMergeAsync(dialogIndex)
            };

            await Task.WhenAll(tasks);
        }

        public Task DeleteDialogAsync(string clientId, string dialogId)
        {
            var tasks = new List<Task>
            {
                _clientDialogIndex.DeleteIfExistAsync(clientId, dialogId),
                _clientDialogIndex.DeleteIfExistAsync(ClientDialogEntity.GenerateDialogIndex(dialogId), clientId)
            };
            
            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<IClientDialog>> GetCommonDialogsAsync()
        {
            var indexes = await _commonDialogIndex.GetDataAsync(ClientDialogEntity.GenerateCommonDialogPartitionKey());
            return await _tableStorage.GetDataAsync(indexes);
        }

        private AzureIndex CreateClientDialogIndex(string clientId, string dialogId)
        {
            return AzureIndex.Create(clientId, dialogId, ClientDialogEntity.GeneratePartitionKey(), dialogId);
        }
        
        private AzureIndex CreateDialogIndex(string clientId, string dialogId)
        {
            return AzureIndex.Create(ClientDialogEntity.GenerateDialogIndex(dialogId), clientId, ClientDialogEntity.GeneratePartitionKey(), dialogId);
        }
        
        private AzureIndex CreateCommonDialogIndex(string dialogId)
        {
            return AzureIndex.Create(ClientDialogEntity.GenerateCommonDialogPartitionKey(), dialogId, 
                ClientDialogEntity.GeneratePartitionKey(), dialogId);
        }
    }
}

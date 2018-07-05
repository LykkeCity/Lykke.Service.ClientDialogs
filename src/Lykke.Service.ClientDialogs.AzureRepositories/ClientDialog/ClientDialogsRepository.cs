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
        private readonly INoSQLTableStorage<AzureIndex> _globalDialogIndex;

        public ClientDialogsRepository(INoSQLTableStorage<ClientDialogEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> clientDialogIndex,
            INoSQLTableStorage<AzureIndex> globalDialogIndex)
        {
            _tableStorage = tableStorage;
            _clientDialogIndex = clientDialogIndex;
            _globalDialogIndex = globalDialogIndex;
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
            var indexEntity = CreateGlobalDialogIndex(dialogId);
            return _globalDialogIndex.InsertOrMergeAsync(indexEntity);
        }

        public Task UnAssignGlobalDialogAsync(string dialogId)
        {
            return _globalDialogIndex.DeleteIfExistAsync(ClientDialogEntity.GenerateGlobalDialogPartitionKey(), dialogId);
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
            
            tasks.Add(UnAssignGlobalDialogAsync(dialogId));

            await Task.WhenAll(tasks);
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
            var indexesTask = _clientDialogIndex.GetDataAsync(clientId);
            var globalIndexesTask = _globalDialogIndex.GetDataAsync(ClientDialogEntity.GenerateGlobalDialogPartitionKey());

            await Task.WhenAll(indexesTask, globalIndexesTask);

            var indexes = await indexesTask;
            var globalIndexes = await globalIndexesTask;
            
            var clientDialogs = (await _tableStorage.GetDataAsync(indexes)).ToList();
            var globalDialogs = (await _tableStorage.GetDataAsync(globalIndexes)).ToList();

            clientDialogs.AddRange(globalDialogs.Where(item=> clientDialogs.All(x => x.Id != item.Id)));

            return clientDialogs;
        }

        public async Task<IClientDialog> GetClientDialogAsync(string clientId, string dialogId)
        {
            var dialog = await GetDialogAsync(dialogId);
            
            if (dialog != null && dialog.IsGlobal)
                return dialog;
            
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

        public async Task<IEnumerable<IClientDialog>> GetGlobalDialogsAsync()
        {
            var indexes = await _globalDialogIndex.GetDataAsync(ClientDialogEntity.GenerateGlobalDialogPartitionKey());
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
        
        private AzureIndex CreateGlobalDialogIndex(string dialogId)
        {
            return AzureIndex.Create(ClientDialogEntity.GenerateGlobalDialogPartitionKey(), dialogId, 
                ClientDialogEntity.GeneratePartitionKey(), dialogId);
        }
    }
}

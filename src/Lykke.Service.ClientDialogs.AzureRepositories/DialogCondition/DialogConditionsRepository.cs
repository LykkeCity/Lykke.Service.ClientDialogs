using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition
{
    public class DialogConditionsRepository : IDialogConditionsRepository
    {
        private readonly INoSQLTableStorage<DialogConditionEntity> _tableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _typeIndex;

        public DialogConditionsRepository(
            INoSQLTableStorage<DialogConditionEntity> tableStorage,
            INoSQLTableStorage<AzureIndex> typeIndex)
        {
            _tableStorage = tableStorage;
            _typeIndex = typeIndex;
        }
        
        public async Task AddDialogConditionAsync(IDialogCondition condition)
        {
            var entity = DialogConditionEntity.Create(condition);
            await _tableStorage.InsertOrMergeAsync(entity);
            
            var typeIndex = AzureIndex.Create(DialogConditionEntity.GenerateTypeIndexPartitionKey(condition.Type),
                    entity.Id, DialogConditionEntity.GeneratePartitionKey(condition.DialogId), DialogConditionEntity.GenerateRowKey(entity.Id));
            await _typeIndex.InsertOrMergeAsync(typeIndex);
        }

        public async Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(string dialogId)
        {
            return await _tableStorage.GetDataAsync(DialogConditionEntity.GeneratePartitionKey(dialogId));
        }

        public async Task<IEnumerable<IDialogCondition>> GetDialogConditionsByTypeAsync(DialogConditionType type)
        {
            var indexes = await _typeIndex.GetDataAsync(DialogConditionEntity.GenerateTypeIndexPartitionKey(type));

            return await _tableStorage.GetDataAsync(indexes);
        }

        public async Task<IDialogCondition> GetDialogConditionAsync(string dialogId, string id)
        {
            return await _tableStorage.GetDataAsync(DialogConditionEntity.GeneratePartitionKey(dialogId),
                DialogConditionEntity.GenerateRowKey(id));
        }
        
        public Task DeleteDialogConditionAsync(string dialogId, string id, DialogConditionType type)
        {
            var tasks = new List<Task>
            {
                _tableStorage.DeleteIfExistAsync(DialogConditionEntity.GeneratePartitionKey(dialogId),
                    DialogConditionEntity.GenerateRowKey(id)),
                _typeIndex.DeleteIfExistAsync(DialogConditionEntity.GenerateTypeIndexPartitionKey(type), id)
            };
            
            return Task.WhenAll(tasks);
        }

        public async Task DeleteDialogConditionsAsync(string dialogId)
        {
            var conditions = (await GetDialogConditionsAsync(dialogId)).ToList();
            var tasks = new List<Task>();

            foreach (var condition in conditions)
            {
                tasks.Add(_tableStorage.DeleteIfExistAsync(DialogConditionEntity.GeneratePartitionKey(dialogId), DialogConditionEntity.GenerateRowKey(condition.Id)));
                tasks.Add(_typeIndex.DeleteIfExistAsync(DialogConditionEntity.GenerateTypeIndexPartitionKey(condition.Type), condition.Id));
            }

            await Task.WhenAll(tasks);
        }
    }
}

using System;
using System.Collections.Generic;
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
            INoSQLTableStorage<AzureIndex> typeIndex
            )
        {
            _tableStorage = tableStorage;
            _typeIndex = typeIndex;
        }
        
        public Task AddDialogConditionAsync(IDialogCondition condition)
        {
            var entity = DialogConditionEntity.Create(condition);
            var index = AzureIndex.Create(DialogConditionEntity.GenerateIndexPartitionKey(condition.DialogId), condition.DialogId,
                DialogConditionEntity.GeneratePartitionKey(condition.Type), DialogConditionEntity.GenerateRowKey(condition.DialogId));

            var tasks = new List<Task>
            {
                _tableStorage.InsertOrMergeAsync(entity),
                _typeIndex.InsertOrMergeAsync(index)
            };

            return Task.WhenAll(tasks);
        }

        public async Task<IEnumerable<IDialogCondition>> GetDialogConditionsAsync(DialogConditionType type)
        {
            return await _tableStorage.GetDataAsync(DialogConditionEntity.GeneratePartitionKey(type));
        }

        public async Task<IDialogCondition> GetDialogConditionAsync(string dialogId)
        {
            var index = await _typeIndex.GetDataAsync(DialogConditionEntity.GenerateIndexPartitionKey(dialogId),
                dialogId);

            return await _tableStorage.GetDataAsync(index);
        }
        
        public async Task DeleteDialogConditionAsync(string dialogId)
        {
            var index = await _typeIndex.GetDataAsync(DialogConditionEntity.GenerateIndexPartitionKey(dialogId),
                DialogConditionEntity.GenerateRowKey(dialogId));

            var tasks = new List<Task>
            {
                _typeIndex.DeleteIfExistAsync(DialogConditionEntity.GenerateIndexPartitionKey(dialogId), dialogId)
            };
            
            if (index != null)
            {
                var type = Enum.Parse<DialogConditionType>(index.PrimaryPartitionKey);
                tasks.Add(_tableStorage.DeleteIfExistAsync(DialogConditionEntity.GeneratePartitionKey(type),
                    DialogConditionEntity.GenerateRowKey(dialogId)));
            }

            await Task.WhenAll(tasks);
        }
    }
}

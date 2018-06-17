using System;
using Lykke.Service.ClientDialogs.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition
{
    public class DialogConditionEntity : TableEntity, IDialogCondition
    {
        public string Id { get; set; }
        public string DialogId { get; set; }
        public DialogConditionType Type { get; set; }
        public string Data { get; set; }

        internal static string GeneratePartitionKey(string dialogId) => dialogId;
        internal static string GenerateTypeIndexPartitionKey(DialogConditionType type) => type.ToString();
        internal static string GenerateRowKey(string id) => id;

        internal static DialogConditionEntity Create(IDialogCondition condition)
        {
            string id = string.IsNullOrEmpty(condition.Id)
                ? Guid.NewGuid().ToString()
                : condition.Id;
            
            return new DialogConditionEntity
            {
                PartitionKey = GeneratePartitionKey(condition.DialogId),
                RowKey = GenerateRowKey(id),
                Id = id,
                DialogId = condition.DialogId,
                Type = condition.Type,
                Data = condition.Data
            };
        }
    }
}

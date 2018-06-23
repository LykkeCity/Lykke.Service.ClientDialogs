using Lykke.Service.ClientDialogs.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientDialogs.AzureRepositories.DialogCondition
{
    public class DialogConditionEntity : TableEntity, IDialogCondition
    {
        public string DialogId { get; set; }
        public DialogConditionType Type { get; set; }
        public string Data { get; set; }

        internal static string GeneratePartitionKey(DialogConditionType type) => type.ToString();
        internal static string GenerateIndexPartitionKey(string dialogId) => $"Index_{dialogId}";
        internal static string GenerateRowKey(string dialogId) => dialogId;

        internal static DialogConditionEntity Create(IDialogCondition condition)
        {
            return new DialogConditionEntity
            {
                PartitionKey = GeneratePartitionKey(condition.Type),
                RowKey = GenerateRowKey(condition.DialogId),
                DialogId = condition.DialogId,
                Type = condition.Type,
                Data = condition.Data
            };
        }
    }
}

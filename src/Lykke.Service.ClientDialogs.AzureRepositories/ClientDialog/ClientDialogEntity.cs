using System;
using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.AzureRepositories.ClientDialog
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateAlways)]
    public class ClientDialogEntity : AzureTableEntity, IClientDialog
    {
        public string Id { get; set; }
        public DialogType Type { get; set; }
        public DialogConditionType? ConditionType { get; set; }
        public bool IsGlobal { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DialogAction[] Actions { get; set; }
        
        internal static string GenerateGlobalDialogPartitionKey() => "GlobalDialog";
        
        internal static string GeneratePartitionKey() => "Dialog";
        internal static string GenerateDialogIndex(string dialogId) => $"DialogIndex_{dialogId}";

        internal static string GenerateRowKey(string dialogId) => dialogId;

        public static ClientDialogEntity Create(IClientDialog dialog)
        {
            string id = string.IsNullOrEmpty(dialog.Id) 
                ? Guid.NewGuid().ToString() 
                : dialog.Id;
            
            return new ClientDialogEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                Id = id,
                Type = dialog.Type,
                ConditionType = dialog.ConditionType,
                IsGlobal = dialog.IsGlobal,
                Header = dialog.Header,
                Text = dialog.Text,
                Actions = dialog.Actions
            };
        }
    }
}

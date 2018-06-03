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
        public string ClientId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DialogAction[] Actions { get; set; }
        
        internal static string GeneratePartitionKey(string clientId) => string.IsNullOrEmpty(clientId) ? "GlobalDialog" : clientId;

        internal static string GenerateRowKey(string dialogId) => dialogId;

        public static ClientDialogEntity Create(IClientDialog dialog)
        {
            return new ClientDialogEntity
            {
                PartitionKey = GeneratePartitionKey(dialog.ClientId),
                RowKey = GenerateRowKey(dialog.Id),
                Id = dialog.Id,
                Type = dialog.Type,
                ClientId = dialog.ClientId,
                Header = dialog.Header,
                Text = dialog.Text,
                Actions = dialog.Actions
            };
        }
    }
}

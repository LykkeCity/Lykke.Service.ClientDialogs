using Lykke.Service.ClientDialogs.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.ClientDialogs.AzureRepositories.ClientDialogSubmit
{
    public class ClientDialogSubmitEntity : TableEntity, IClientDialogSubmit
    {
        public string ClientId { get; set; }
        public string DialogId { get; set; }
        public string ActionId { get; set; }
        
        internal static string GeneratePartitionKey(string clientId) => clientId;

        internal static string GenerateRowKey(string dialogId, string actionId) => $"{dialogId}_{actionId}";

        public static ClientDialogSubmitEntity Create(string clientId, string dialogId, string actionId)
        {
            return new ClientDialogSubmitEntity
            {
                PartitionKey = GeneratePartitionKey(clientId),
                RowKey = GenerateRowKey(dialogId, actionId),
                ClientId = clientId,
                DialogId = dialogId,
                ActionId = actionId
            };
        }
    }
}

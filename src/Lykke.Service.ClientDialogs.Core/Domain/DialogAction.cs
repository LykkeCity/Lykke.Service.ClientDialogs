using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.Serializers;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    [ValueSerializer(typeof(JsonStorageValueSerializer))]
    public class DialogAction
    {
        public string Id { get; set; }
        public ActionType Type { get; set; }
        public string Text { get; set; }
    }
}

namespace Lykke.Service.ClientDialogs.Client.Models
{
    public class DialogConditionModel
    {
        public string Id { get; set; }
        public string DialogId { get; set; }
        public DialogConditionType Type { get; set; }
        public string Data { get; set; }
    }
}

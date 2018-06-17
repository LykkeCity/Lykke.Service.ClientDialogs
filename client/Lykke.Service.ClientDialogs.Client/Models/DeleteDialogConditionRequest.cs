namespace Lykke.Service.ClientDialogs.Client.Models
{
    public class DeleteDialogConditionRequest
    {
        public string DialogId { get; set; }
        public string ConditionId { get; set; }
        public DialogConditionType Type { get; set; }
    }
}

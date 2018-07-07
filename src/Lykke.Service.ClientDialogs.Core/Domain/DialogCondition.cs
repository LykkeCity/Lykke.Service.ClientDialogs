namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public class DialogCondition : IDialogCondition
    {
        public string DialogId { get; set; }
        public DialogConditionType Type { get; set; }
        public string Data { get; set; }
    }
}

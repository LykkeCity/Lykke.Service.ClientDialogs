namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IDialogCondition
    {
        string DialogId { get; }
        DialogConditionType Type { get; }
        string Data { get; }
    }
}

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IDialogCondition
    {
        string Id { get; }
        string DialogId { get; }
        DialogConditionType Type { get; }
        string Data { get; }
    }
}

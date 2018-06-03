namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialogSubmit
    {
        string ClientId { get; }
        string DialogId { get; }
        string ActionId { get; }
    }
}

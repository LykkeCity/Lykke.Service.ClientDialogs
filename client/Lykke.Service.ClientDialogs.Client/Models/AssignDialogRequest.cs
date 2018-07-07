namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Assign dialog to the client request
    /// </summary>
    public class AssignDialogRequest
    {
        public string ClientId { get; set; }
        public string DialogId { get; set; }
    }
}

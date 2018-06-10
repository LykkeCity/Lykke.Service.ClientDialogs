namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Request to submit dialog
    /// </summary>
    public class SubmitDialogRequest
    {
        
        /// <summary>
        /// ClientId who submits the dialog
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Action id
        /// </summary>
        public string DialogId { get; set; }
        
        /// <summary>
        /// Action id
        /// </summary>
        public string ActionId { get; set; }
    }
}

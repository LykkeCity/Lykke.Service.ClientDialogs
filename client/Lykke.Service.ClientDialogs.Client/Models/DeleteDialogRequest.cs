namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Delete dialog request
    /// </summary>
    public class DeleteDialogRequest
    {
        /// <summary>
        /// Client Id
        /// <remarks>null for common dialog</remarks>
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Dialog id
        /// </summary>
        public string DialogId { get; set; }
    }
}

namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Dialog action (button or checkbox)
    /// </summary>
    public class DialogActionModel
    {
        /// <summary>
        /// Action id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Action type (button, checkbox)
        /// </summary>
        public ActionType Type { get; set; }
        
        /// <summary>
        /// Action text
        /// </summary>
        public string Text { get; set; }
    }
}

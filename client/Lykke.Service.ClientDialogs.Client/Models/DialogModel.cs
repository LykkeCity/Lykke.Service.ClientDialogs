using System;

namespace Lykke.Service.ClientDialogs.Client.Models
{
    /// <summary>
    /// Client dialog
    /// </summary>
    public class DialogModel
    {
        /// <summary>
        /// Dialog id
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Dialog type
        /// </summary>
        public DialogType Type { get; set; }
        
        /// <summary>
        /// Dialog type
        /// </summary>
        public DialogConditionType? ConditionType { get; set; }
        
        /// <summary>
        /// Is the dialog assigned as a global dialog for all clients
        /// </summary>
        public bool IsGlobal { get; set; }
        
        /// <summary>
        /// Dialog title
        /// </summary>
        public string Header { get; set; }
        
        /// <summary>
        /// Dialog text
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// List of dialog actions (buttons or checkbox) for submit
        /// </summary>
        public DialogActionModel[] Actions { get; set; } = Array.Empty<DialogActionModel>();
    }
}

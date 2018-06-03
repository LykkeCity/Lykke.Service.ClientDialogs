using System;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.ClientDialogs.Core.Domain;

namespace Lykke.Service.ClientDialogs.Models
{
    public class ClientDialogModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public DialogType Type { get; set; }
        public string ClientId { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Text { get; set; }
        public DialogActionModel[] Actions { get; set; } = Array.Empty<DialogActionModel>();
    }

    public class DialogActionModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public ActionType Type { get; set; }
        [Required]
        public string Text { get; set; }
    }
}

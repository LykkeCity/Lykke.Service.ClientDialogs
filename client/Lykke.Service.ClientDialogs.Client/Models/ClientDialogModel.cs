using System;

namespace Lykke.Service.ClientDialogs.Client.Models
{
    public class ClientDialogModel
    {
        public string Id { get; set; }
        public DialogType Type { get; set; }
        public string ClientId { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DialogActionModel[] Actions { get; set; } = Array.Empty<DialogActionModel>();
    }

    public class DialogActionModel
    {
        public string Id { get; set; }
        public ActionType Type { get; set; }
        public string Text { get; set; }
    }
}

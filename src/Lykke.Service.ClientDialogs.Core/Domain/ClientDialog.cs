using System;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public class ClientDialog : IClientDialog
    {
        public string Id { get; set; }
        public DialogType Type { get; set; }
        public bool IsCommon { get; set; }
        public string Header { get; set; }
        public string Text { get; set; }
        public DialogAction[] Actions { get; set; } = Array.Empty<DialogAction>();
    }
}

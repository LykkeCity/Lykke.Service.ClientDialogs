using System;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialog
    {
        string Id { get; set; }
        DialogType Type { get; set; }
        bool IsCommon { get; set; }
        string Header { get; set; }
        string Text { get; set; }
        DialogAction[] Actions { get; set; }
    }
}

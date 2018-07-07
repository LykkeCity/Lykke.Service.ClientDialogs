using System;

namespace Lykke.Service.ClientDialogs.Core.Domain
{
    public interface IClientDialog
    {
        string Id { get; }
        DialogType Type { get; }
        DialogConditionType? ConditionType { get; }
        bool IsGlobal { get; }
        string Header { get; }
        string Text { get; }
        DialogAction[] Actions { get; }
    }
}

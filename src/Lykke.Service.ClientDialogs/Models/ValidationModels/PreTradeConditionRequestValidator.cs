using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class PreTradeConditionRequestValidator : AbstractValidator<PreTradeConditionRequest>
    {
        public PreTradeConditionRequestValidator()
        {
            RuleFor(x => x.DialogId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.DialogId)} value");

            RuleFor(x => x.AssetId)
                .NotEmpty();
        }
    }
}

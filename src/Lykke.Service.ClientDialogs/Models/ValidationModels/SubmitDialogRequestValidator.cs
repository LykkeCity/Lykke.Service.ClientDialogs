using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class SubmitDialogRequestValidator : AbstractValidator<SubmitDialogRequest>
    {
        public SubmitDialogRequestValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ClientId)} value");
            
            RuleFor(x => x.DialogId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.DialogId)} value");
            
            RuleFor(x => x.ActionId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ActionId)} value");
        }
    }
}

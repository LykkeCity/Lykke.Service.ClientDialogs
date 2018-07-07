using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class AssignDialogRequestValidator : AbstractValidator<AssignDialogRequest>
    {
        public AssignDialogRequestValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ClientId)} value");
            
            RuleFor(x => x.DialogId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.DialogId)} value");
        }
    }
}

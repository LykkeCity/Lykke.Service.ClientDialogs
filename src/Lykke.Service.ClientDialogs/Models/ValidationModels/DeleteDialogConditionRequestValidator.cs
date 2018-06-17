using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class DeleteDialogConditionRequestValidator : AbstractValidator<DeleteDialogConditionRequest>
    {
        public DeleteDialogConditionRequestValidator()
        {
            RuleFor(x => x.DialogId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.DialogId)} value");
            
            RuleFor(x => x.ConditionId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ConditionId)} value");

            RuleFor(x => x.Type)
                .NotNull();
        }
    }
}

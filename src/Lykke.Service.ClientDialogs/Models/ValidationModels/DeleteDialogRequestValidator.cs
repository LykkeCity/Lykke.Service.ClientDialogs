using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class DeleteDialogRequestValidator : AbstractValidator<DeleteDialogRequest>
    {
        public DeleteDialogRequestValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ClientId)} value");
            
            RuleFor(x => x.DialogId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ClientId)} value");
        }
    }
}

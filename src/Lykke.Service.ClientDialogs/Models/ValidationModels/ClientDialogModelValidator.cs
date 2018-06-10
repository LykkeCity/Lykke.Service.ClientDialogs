using System.Security.Cryptography.X509Certificates;
using Common;
using FluentValidation;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    public class ClientDialogModelValidator : AbstractValidator<ClientDialogModel>
    {
        public ClientDialogModelValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(s => s.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.Id)} value");
            
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .Must(x => x.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.ClientId)} value");
            
            RuleFor(x => x.Header)
                .NotEmpty();
            
            RuleFor(x => x.Text)
                .NotEmpty();

            RuleFor(x => x.Actions)
                .NotEmpty()
                .WithMessage("Specify at least one action");
        }
    }
}

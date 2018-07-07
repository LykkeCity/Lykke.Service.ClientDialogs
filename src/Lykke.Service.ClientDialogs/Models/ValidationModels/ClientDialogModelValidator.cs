using System.Linq;
using Common;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.ClientDialogs.Client.Models;

namespace Lykke.Service.ClientDialogs.Models.ValidationModels
{
    [UsedImplicitly]
    public class ClientDialogModelValidator : AbstractValidator<DialogModel>
    {
        public ClientDialogModelValidator()
        {
            RuleSet("new", () =>
            {
                RuleFor(x => x.Header)
                    .NotEmpty();
            
                RuleFor(x => x.Text)
                    .NotEmpty();

                RuleFor(x => x.Actions)
                    .NotEmpty()
                    .WithMessage("Specify at least one action")
                    .Must(actions => actions.All(item => item.Id.IsValidPartitionOrRowKey()))
                    .WithMessage("Actions has a record with invalid Id");
            });
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(s => s.IsValidPartitionOrRowKey())
                .WithMessage(x => $"Invalid {nameof(x.Id)} value");
            
        }
    }
}

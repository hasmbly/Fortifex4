using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Wallets.Commands.CreatePersonalWallet
{
    public class CreatePersonalWalletCommandValidator : AbstractValidator<CreatePersonalWalletCommand>
    {
        private readonly IFortifex4DBContext _context;

        public CreatePersonalWalletCommandValidator(IFortifex4DBContext context)
        {
            _context = context;

            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Wallet name is required.")
                .MaximumLength(200).WithMessage("Wallet name must not exceed 200 characters.");

            RuleFor(v => v.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters.");

            RuleFor(v => v)
                .MustAsync(BeUniqueName).WithMessage("The specified wallet name already exists.");
        }

        public async Task<bool> BeUniqueName(CreatePersonalWalletCommand command, CancellationToken cancellationToken)
        {
            bool isUnique = true;

            var owner = await _context.Owners
                .Where(x =>
                    x.MemberUsername == command.MemberUsername &&
                    x.ProviderType == ProviderType.Personal)
                .Include(x => x.Wallets)
                .SingleOrDefaultAsync(cancellationToken);

            if (owner != null)
            {
                if (owner.Wallets.Any(x => x.Name == command.Name))
                {
                    isUnique = false;
                }
            }

            return isUnique;
        }
    }
}
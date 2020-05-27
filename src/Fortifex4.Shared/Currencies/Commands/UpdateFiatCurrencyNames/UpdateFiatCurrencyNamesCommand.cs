using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Fiat;
using Fortifex4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencyNames
{
    public class UpdateFiatCurrencyNamesCommand : IRequest<UpdateFiatCurrencyNamesResult>
    {
    }

    public class UpdateFiatCurrencyNamesCommandHandler : IRequestHandler<UpdateFiatCurrencyNamesCommand, UpdateFiatCurrencyNamesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFiatService _fiatService;

        public UpdateFiatCurrencyNamesCommandHandler(IFortifex4DBContext context, IFiatService fiatService)
        {
            _context = context;
            _fiatService = fiatService;
        }

        public async Task<UpdateFiatCurrencyNamesResult> Handle(UpdateFiatCurrencyNamesCommand request, CancellationToken cancellationToken)
        {
            var result = new UpdateFiatCurrencyNamesResult();

            var fiatCurrencyCollection = await _fiatService.GetFiatCurrencyCollectionAsync();

            foreach (var fiatCurrency in fiatCurrencyCollection.Currencies)
            {
                var existingFiatCurrency = await _context.Currencies
                    .Where(x => x.Symbol == fiatCurrency.Symbol && x.CurrencyType == CurrencyType.Fiat)
                    .SingleOrDefaultAsync(cancellationToken);

                if (existingFiatCurrency != null)
                {
                    existingFiatCurrency.Name = fiatCurrency.Name;

                    result.Currencies.Add(new CurrencyDTO
                    {
                        CurrencyID = existingFiatCurrency.CurrencyID,
                        Symbol = existingFiatCurrency.Symbol,
                        Name = existingFiatCurrency.Name
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
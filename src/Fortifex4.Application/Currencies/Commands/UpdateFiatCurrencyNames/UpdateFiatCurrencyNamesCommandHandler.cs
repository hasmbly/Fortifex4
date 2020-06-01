using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Fiat;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyNames;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencyNames
{
    public class UpdateFiatCurrencyNamesCommandHandler : IRequestHandler<UpdateFiatCurrencyNamesRequest, UpdateFiatCurrencyNamesResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFiatService _fiatService;

        public UpdateFiatCurrencyNamesCommandHandler(IFortifex4DBContext context, IFiatService fiatService)
        {
            _context = context;
            _fiatService = fiatService;
        }

        public async Task<UpdateFiatCurrencyNamesResponse> Handle(UpdateFiatCurrencyNamesRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateFiatCurrencyNamesResponse();

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

            result.IsSuccessful = true;

            return result;
        }
    }
}
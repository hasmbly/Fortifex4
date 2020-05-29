using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Currencies.Commands.UpdateFiatCurrencyCoinMarketCapIDs
{
    public class UpdateFiatCurrencyCoinMarketCapIDsCommandHandler : IRequestHandler<UpdateFiatCurrencyCoinMarketCapIDsRequest, UpdateFiatCurrencyCoinMarketCapIDsResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public UpdateFiatCurrencyCoinMarketCapIDsCommandHandler(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<UpdateFiatCurrencyCoinMarketCapIDsResponse> Handle(UpdateFiatCurrencyCoinMarketCapIDsRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateFiatCurrencyCoinMarketCapIDsResponse();

            var fiatCurrencyMapJSON = _fileService.ReadJSONFile<FiatCurrencyMapJSON>(FilePath.FiatCurrencyMapJSON);

            if (fiatCurrencyMapJSON.status.error_code == 0)
            {
                foreach (var fiatCurrencyJSON in fiatCurrencyMapJSON.data)
                {
                    string symbol = !string.IsNullOrEmpty(fiatCurrencyJSON.symbol) ? fiatCurrencyJSON.symbol : fiatCurrencyJSON.code;

                    var currency = await _context.Currencies
                        .Where(x =>
                            x.CurrencyType == CurrencyType.Fiat &&
                            x.Symbol == symbol)
                        .SingleOrDefaultAsync(cancellationToken);

                    if (currency != null)
                    {
                        currency.CoinMarketCapID = fiatCurrencyJSON.id;

                        await _context.SaveChangesAsync(cancellationToken);

                        result.Currencies.Add(new CurrencyDTO
                        {
                            CurrencyID = currency.CurrencyID,
                            Symbol = symbol,
                            CoinMarketCapID = fiatCurrencyJSON.id
                        });
                    }
                }
            }

            return result;
        }
    }
}
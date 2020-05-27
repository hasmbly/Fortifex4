using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Application.System.Commands.SeedFiatCurrencies
{
    public class SeedFiatCurrenciesCommand : IRequest<SeedFiatCurrenciesResult>
    {
    }

    public class SeedFiatCurrenciesCommandHandler : IRequestHandler<SeedFiatCurrenciesCommand, SeedFiatCurrenciesResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public SeedFiatCurrenciesCommandHandler(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<SeedFiatCurrenciesResult> Handle(SeedFiatCurrenciesCommand request, CancellationToken cancellationToken)
        {
            var result = new SeedFiatCurrenciesResult();

            var fiatCurrencyMapJSON = _fileService.ReadJSONFile<FiatCurrencyMapJSON>(FilePath.FiatCurrencyMapJSON);

            if (fiatCurrencyMapJSON.status.error_code == 0)
            {
                Blockchain fiatBlockchain = new Blockchain
                {
                    BlockchainID = BlockchainID.Fiat,
                    Symbol = BlockchainSymbol.Fiat,
                    Name = BlockchainName.Fiat
                };

                foreach (var fiatCurrencyJSON in fiatCurrencyMapJSON.data)
                {
                    fiatBlockchain.Currencies.Add(new Currency
                    {
                        BlockchainID = fiatBlockchain.BlockchainID,
                        Symbol = fiatCurrencyJSON.symbol,
                        Name = fiatCurrencyJSON.name,
                        CoinMarketCapID = fiatCurrencyJSON.id,
                        Rank = 0,
                        CurrencyType = CurrencyType.Fiat,
                        IsShownInTradePair = fiatCurrencyJSON.symbol == CurrencySymbol.USD,
                        IsForPreferredOption = true
                    });
                }

                if (!_context.Blockchains.Any(x => x.BlockchainID == BlockchainID.Fiat))
                {
                    _context.Blockchains.Add(fiatBlockchain);
                    await _context.SaveChangesAsync(cancellationToken);

                    result.FiatCurrenciesAdded = fiatBlockchain.Currencies.Count;
                }

                result.IsSuccessful = true;
            }
            else
            {
                result.IsSuccessful = false;
                result.ErrorMessage = fiatCurrencyMapJSON.status.error_message;
            }

            return result;
        }
    }
}
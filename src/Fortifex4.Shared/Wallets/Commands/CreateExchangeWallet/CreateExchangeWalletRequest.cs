using MediatR;

namespace Fortifex4.Shared.Wallets.Commands.CreateExchangeWallet
{
    public class CreateExchangeWalletRequest : IRequest<CreateExchangeWalletResponse>
    {
        public int OwnerID { get; set; }
        public int CurrencyID { get; set; }
        public decimal? StartingBalance { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Shared.Members.Commands.UpdatePreferredCoinCurrency
{
    public class UpdatePreferredCoinCurrencyRequest : IRequest<UpdatePreferredCoinCurrencyResponse>
    {
        public string MemberUsername { get; set; }
        public int PreferredCoinCurrencyID { get; set; }
    }
}
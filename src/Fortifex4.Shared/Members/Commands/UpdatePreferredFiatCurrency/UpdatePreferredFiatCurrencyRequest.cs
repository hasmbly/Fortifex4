using MediatR;

namespace Fortifex4.Shared.Members.Commands.UpdatePreferredFiatCurrency
{
    public class UpdatePreferredFiatCurrencyRequest : IRequest<UpdatePreferredFiatCurrencyResponse>
    {
        public string MemberUsername { get; set; }
        public int PreferredFiatCurrencyID { get; set; }
    }
}
using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetCurrency
{
    public class GetCurrencyRequest : IRequest<GetCurrencyResponse>
    {
        public int CurrencyID { get; set; }
    }
}
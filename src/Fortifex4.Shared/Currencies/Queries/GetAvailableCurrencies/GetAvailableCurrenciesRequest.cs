using MediatR;

namespace Fortifex4.Shared.Currencies.Queries.GetAvailableCurrencies
{
    public class GetAvailableCurrenciesRequest : IRequest<GetAvailableCurrenciesResponse>
    {
        public int OwnerID { get; set; }
    }
}
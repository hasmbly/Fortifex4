using MediatR;

namespace Fortifex4.Shared.Pockets.Queries.GetPocket
{
    public class GetPocketRequest : IRequest<GetPocketResponse>
    {
        public int PocketID { get; set; }
        public string Address { get; set; }
    }
}
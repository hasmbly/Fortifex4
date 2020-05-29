using MediatR;

namespace Fortifex4.Shared.Owners.Queries.GetOwner
{
    public class GetOwnerRequest : IRequest<GetOwnerResponse>
    {
        public int OwnerID { get; set; }
    }
}
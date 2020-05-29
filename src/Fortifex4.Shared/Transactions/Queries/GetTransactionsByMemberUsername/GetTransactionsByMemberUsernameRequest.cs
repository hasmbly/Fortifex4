using MediatR;

namespace Fortifex4.Shared.Transactions.Queries.GetTransactionsByMemberUsername
{
    public class GetTransactionsByMemberUsernameRequest : IRequest<GetTransactionsByMemberUsernameResponse>
    {
        public string MemberUsername { get; set; }
    }
}
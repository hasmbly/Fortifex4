using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Members.Queries.GetMember
{
    public class GetMemberRequest : IRequest<GetMemberResponse>
    {
        [Required(ErrorMessage = "Please enter your MemberUsername.")]
        public string MemberUsername { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Members.Commands.CreateMember
{
    public class CreateMemberRequest : IRequest<CreateMemberResponse>
    {
        [Required(ErrorMessage = "Please enter your e-mail address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string MemberUsername { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }
    }
}
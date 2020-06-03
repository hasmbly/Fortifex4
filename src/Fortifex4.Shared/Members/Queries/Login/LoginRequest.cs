using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Members.Queries.Login
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        [Required(ErrorMessage = "Please enter your e-mail address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string MemberUsername { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; } = string.Empty;
    }
}
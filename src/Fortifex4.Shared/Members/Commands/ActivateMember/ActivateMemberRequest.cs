using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Members.Commands.ActivateMember
{
    public class ActivateMemberRequest : IRequest<ActivateMemberResponse>
    {
        [Required(ErrorMessage = "Please enter your Activation Code.")]
        public Guid ActivationCode { get; set; }
    }
}
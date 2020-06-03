using System;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Members.Commands.UpdateMember
{
    public class UpdateMemberRequest : IRequest<UpdateMemberResponse>
    {
        [Required]
        public string MemberUsername { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int GenderID { get; set; }

        [Required]
        public int RegionID { get; set; }
    }
}
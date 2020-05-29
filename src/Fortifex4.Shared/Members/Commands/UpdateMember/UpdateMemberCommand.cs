using System;
using MediatR;

namespace Fortifex4.Shared.Members.Commands.UpdateMember
{
    public class UpdateMemberRequest : IRequest<UpdateMemberResponse>
    {
        public string MemberUsername { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }
        public int RegionID { get; set; }
    }
}
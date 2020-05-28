using System;

namespace Fortifex4.Shared.Contributors.Commands.CreateContributors
{
    public class ContributorDTO
    {
        public string MemberUsername { get; set; }
        public int ProjectID { get; set; }
        public Guid InvitationCode { get; set; }
    }
}
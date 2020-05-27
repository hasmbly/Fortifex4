using System;
using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;

namespace Fortifex4.Domain.Entities
{
    public class Contributor : AuditableEntity
    {
        public int ContributorID { get; set; }
        public int ProjectID { get; set; }
        public string MemberUsername { get; set; }
        public Guid InvitationCode { get; set; }
        public InvitationStatus InvitationStatus { get; set; }

        public Project Project { get; set; }
    }
}
using Fortifex4.Domain.Enums;

namespace Fortifex4.Application.Projects.Queries.GetProject
{
    public class ContributorDTO
    {
        public int ContributorID { get; set; }
        public string MemberUsername { get; set; }
        public int ProjectID { get; set; }
        public InvitationStatus InvitationStatus { get; set; }

        public string InvitationStatusDisplayText
        {
            get
            {
                return this.InvitationStatus switch
                {
                    InvitationStatus.Invited => "Invited",
                    InvitationStatus.Accepted => "Accepted",
                    InvitationStatus.Rejected => "Rejected",
                    _ => null,
                };
            }
        }
    }
}
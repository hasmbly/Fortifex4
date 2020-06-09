using MediatR;

namespace Fortifex4.Shared.Members.Commands.UpdatePreferredTimeFrame
{
    public class UpdatePreferredTimeFrameRequest : IRequest<UpdatePreferredTimeFrameResponse>
    {
        public string MemberUsername { get; set; }
        public int PreferredTimeFrameID { get; set; }
    }
}
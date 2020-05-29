using MediatR;

namespace Fortifex4.Shared.Members.Queries.GetPreferences
{
    public class GetPreferencesRequest : IRequest<GetPreferencesResponse>
    {
        public string MemberUsername { get; set; }
    }
}
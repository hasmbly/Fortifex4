using MediatR;

namespace Fortifex4.Shared.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsRequest : IRequest<GetMyProjectsResponse>
    {
        public string MemberUsername { get; set; }
    }
}
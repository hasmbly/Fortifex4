using MediatR;

namespace Fortifex4.Application.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsQuery : IRequest<GetMyProjectsResult>
    {
        public string MemberUsername { get; set; }
    }
}
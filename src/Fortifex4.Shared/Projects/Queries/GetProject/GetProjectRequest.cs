using MediatR;

namespace Fortifex4.Shared.Projects.Queries.GetProject
{
    public class GetProjectRequest : IRequest<GetProjectResponse>
    {
        public int ProjectID { get; set; }
        public string IsExistProjectByMemberUsername { get; set; }
    }
}
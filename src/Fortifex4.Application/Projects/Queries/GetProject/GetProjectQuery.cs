using MediatR;

namespace Fortifex4.Application.Projects.Queries.GetProject
{
    public class GetProjectQuery : IRequest<GetProjectResult>
    {
        public int ProjectID { get; set; }
        public string IsExistProjectByMemberUsername { get; set; }
    }
}
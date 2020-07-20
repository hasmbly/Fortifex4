using MediatR;

namespace Fortifex4.Shared.ProjectStatusLogs.Queries.GetProjectStatusLogsByProjectID
{
    public class GetProjectStatusLogsByProjectIDRequest : IRequest<GetProjectStatusLogsByProjectIDResponse>
    {
        public int ProjectID { get; set; }
    }
}
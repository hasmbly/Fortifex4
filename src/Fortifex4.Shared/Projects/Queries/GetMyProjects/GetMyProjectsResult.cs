using System.Collections.Generic;

namespace Fortifex4.Application.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsResult
    {
        public IList<ProjectDTO> Projects { get; set; }

        public GetMyProjectsResult()
        {
            this.Projects = new List<ProjectDTO>();
        }
    }
}
using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Projects.Queries.GetMyProjects
{
    public class GetMyProjectsResponse : GeneralResponse
    {
        public IList<ProjectDTO> Projects { get; set; }

        public GetMyProjectsResponse()
        {
            this.Projects = new List<ProjectDTO>();
        }
    }
}
using System.Collections.Generic;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Projects.Queries.GetProjectsConfirmation
{
    public class GetProjectsConfirmationResponse : GeneralResponse
    {
        public IList<ProjectDTO> Projects { get; set; }

        public GetProjectsConfirmationResponse()
        {
            this.Projects = new List<ProjectDTO>();
        }
    }
}
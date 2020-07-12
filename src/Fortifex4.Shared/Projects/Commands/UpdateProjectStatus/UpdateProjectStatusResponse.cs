using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Projects.Commands.UpdateProjectStatus
{
    public class UpdateProjectStatusResponse : GeneralResponse
    {
        public int ProjectID { get; set; }
        public ProjectStatus NewProjectStatus { get; set; }
    }
}
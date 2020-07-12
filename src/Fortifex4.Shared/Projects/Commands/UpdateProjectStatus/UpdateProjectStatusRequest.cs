using Fortifex4.Domain.Enums;
using MediatR;

namespace Fortifex4.Shared.Projects.Commands.UpdateProjectStatus
{
    public class UpdateProjectStatusRequest : IRequest<UpdateProjectStatusResponse>
    {
        public int ProjectID { get; set; }
        public ProjectStatus NewProjectStatus { get; set; }
        public string Comment { get; set; }
    }
}
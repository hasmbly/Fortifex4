using MediatR;

namespace Fortifex4.Shared.Projects.Commands.UpdateProjects
{
    public class UpdateProjectsRequest : IRequest<UpdateProjectsResponse>
    {
        public int ProjectID { get; set; }
        public int ProjectBlockchainID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectWalletAddress { get; set; }
    }
}
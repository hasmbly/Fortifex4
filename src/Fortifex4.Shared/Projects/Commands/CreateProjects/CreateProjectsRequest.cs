using MediatR;

namespace Fortifex4.Shared.Projects.Commands.CreateProjects
{
    public class CreateProjectsRequest : IRequest<CreateProjectsResponse>
    {
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WalletAddress { get; set; }
    }
}
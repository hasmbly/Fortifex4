using MediatR;

namespace Fortifex4.Application.Projects.Commands.CreateProjects
{
    public class CreateProjectsCommand : IRequest<CreateProjectsResult>
    {
        public string MemberUsername { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string WalletAddress { get; set; }
    }
}
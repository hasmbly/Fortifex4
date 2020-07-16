using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Fortifex4.Shared.Projects.Commands.CreateProjects
{
    public class CreateProjectsRequest : IRequest<CreateProjectsResponse>
    {
        [Required]
        public string MemberUsername { get; set; }

        [Required]
        public int BlockchainID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string WalletAddress { get; set; }
    }
}
using MediatR;
using System.Collections.Generic;

namespace Fortifex4.Shared.Contributors.Commands.CreateContributors
{
    public class CreateContributorsRequest : IRequest<CreateContributorsResponse>
    {
        public IList<string> MemberUsername { get; set; }
        public int ProjectID { get; set; }
    }
}
using MediatR;
using System.Collections.Generic;

namespace Fortifex4.Application.Contributors.Commands.CreateContributors
{
    public class CreateContributorsCommand : IRequest<CreateContributorsResult>
    {
        public IList<string> MemberUsername { get; set; }
        public int ProjectID { get; set; }
    }
}
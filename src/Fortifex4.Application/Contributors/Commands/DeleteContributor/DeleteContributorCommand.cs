using MediatR;

namespace Fortifex4.Application.Contributors.Commands.DeleteContributor
{
    public class DeleteContributorCommand : IRequest<DeleteContributorResult>
    {
        public int ContributorID { get; set; }
    }
}
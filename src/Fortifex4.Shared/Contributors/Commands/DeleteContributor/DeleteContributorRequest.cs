using MediatR;

namespace Fortifex4.Shared.Contributors.Commands.DeleteContributor
{
    public class DeleteContributorRequest : IRequest<DeleteContributorResponse>
    {
        public int ContributorID { get; set; }
    }
}
using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
using Fortifex4.Shared.Contributors.Commands.DeleteContributor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.DeleteContributor
{
    public class DeleteContributorCommandHandler : IRequestHandler<DeleteContributorRequest, DeleteContributorResponse>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteContributorCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteContributorResponse> Handle(DeleteContributorRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteContributorResponse();

            var contributor = await _context.Contributors
                .Where(x => x.ContributorID == request.ContributorID)
                .SingleOrDefaultAsync();

            if (contributor == null)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.ContributorNotFound;

                return result;
            }

            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
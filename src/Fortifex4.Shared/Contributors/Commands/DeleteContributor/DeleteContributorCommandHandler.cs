using Fortifex4.Application.Common.Exceptions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fortifex4.Application.Contributors.Commands.DeleteContributor
{
    class DeleteContributorCommandHandler : IRequestHandler<DeleteContributorCommand, DeleteContributorResult>
    {
        private readonly IFortifex4DBContext _context;

        public DeleteContributorCommandHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<DeleteContributorResult> Handle(DeleteContributorCommand request, CancellationToken cancellationToken)
        {
            var result = new DeleteContributorResult()
            {
                IsSuccessful = false
            };

            var contributor = await _context.Contributors
                .Where(x => x.ContributorID == request.ContributorID)
                .SingleOrDefaultAsync();

            if (contributor == null)
                throw new NotFoundException(nameof(contributor), request.ContributorID);

            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync(cancellationToken);

            result.IsSuccessful = true;

            return result;
        }
    }
}
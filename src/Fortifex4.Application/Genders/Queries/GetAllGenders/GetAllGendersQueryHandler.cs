using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Genders.Queries.GetAllGenders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GetAllGendersQueryHandler : IRequestHandler<GetAllGendersRequest, GetAllGendersResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllGendersQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllGendersResponse> Handle(GetAllGendersRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllGendersResponse();

            var genders = await _context.Genders.ToListAsync(cancellationToken);

            foreach (var gender in genders)
            {
                result.Genders.Add(new GenderDTO
                {
                    GenderID = gender.GenderID,
                    Name = gender.Name
                });
            }

            return result;
        }
    }
}
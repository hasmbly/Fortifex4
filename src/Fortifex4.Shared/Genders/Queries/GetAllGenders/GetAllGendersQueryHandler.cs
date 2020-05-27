using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GetAllGendersQueryHandler : IRequestHandler<GetAllGendersQuery, GetAllGendersResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllGendersQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllGendersResult> Handle(GetAllGendersQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllGendersResult();

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
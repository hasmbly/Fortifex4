using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, GetAllCountriesResult>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllCountriesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllCountriesResult> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
        {
            var result = new GetAllCountriesResult();

            var countries = await _context.Countries.ToListAsync(cancellationToken);

            foreach (var country in countries)
            {
                result.Countries.Add(new CountryDTO
                {
                    CountryCode = country.CountryCode,
                    Name = country.Name
                });
            }

            return result;
        }
    }
}
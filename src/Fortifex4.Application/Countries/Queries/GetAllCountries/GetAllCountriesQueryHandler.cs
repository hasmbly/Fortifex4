using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Countries.Queries.GetAllCountries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Countries.Queries.GetAllCountries
{
    public class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesRequest, GetAllCountriesResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetAllCountriesQueryHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetAllCountriesResponse> Handle(GetAllCountriesRequest request, CancellationToken cancellationToken)
        {
            var result = new GetAllCountriesResponse();

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
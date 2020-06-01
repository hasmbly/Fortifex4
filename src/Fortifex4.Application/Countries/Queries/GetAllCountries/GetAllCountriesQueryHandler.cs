using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.Constants;
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

            if (countries.Count != 0)
            {
                result.IsSuccessful = false;
                result.ErrorMessage = ErrorMessage.CountriesNotFound;

                return result;
            }

            foreach (var country in countries)
            {
                result.Countries.Add(new CountryDTO
                {
                    CountryCode = country.CountryCode,
                    Name = country.Name
                });
            }

            result.IsSuccessful = true;

            return result;
        }
    }
}
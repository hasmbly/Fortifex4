using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fortifex4.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GetAllGendersQueryHandler : IRequestHandler<GetAllGendersQuery, GetAllGendersResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IMapper _mapper;

        public GetAllGendersQueryHandler(IFortifex4DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllGendersResult> Handle(GetAllGendersQuery request, CancellationToken cancellationToken)
        {
            var genders = await _context.Genders.ProjectTo<GenderDTO>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return new GetAllGendersResult { Genders = genders };
        }
    }
}
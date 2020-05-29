using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Shared.System.Commands.SeedMasterData;
using MediatR;

namespace Fortifex4.Application.System.Commands.SeedMasterData
{
    public class SeedMasterDataCommandHandler : IRequestHandler<SeedMasterDataRequest, SeedMasterDataResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public SeedMasterDataCommandHandler(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<SeedMasterDataResponse> Handle(SeedMasterDataRequest request, CancellationToken cancellationToken)
        {
            MasterDataSeeder seeder = new MasterDataSeeder(_context, _fileService);

            var result = await seeder.SeedAllAsync(cancellationToken);

            return result;
        }
    }
}
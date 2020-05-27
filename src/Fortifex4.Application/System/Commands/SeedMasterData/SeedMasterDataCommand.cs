using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using MediatR;

namespace Fortifex4.Application.System.Commands.SeedMasterData
{
    public class SeedMasterDataCommand : IRequest<SeedMasterDataResult>
    {
    }

    public class SeedMasterDataCommandHandler : IRequestHandler<SeedMasterDataCommand, SeedMasterDataResult>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public SeedMasterDataCommandHandler(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<SeedMasterDataResult> Handle(SeedMasterDataCommand request, CancellationToken cancellationToken)
        {
            MasterDataSeeder seeder = new MasterDataSeeder(_context, _fileService);

            SeedMasterDataResult result = await seeder.SeedAllAsync(cancellationToken);

            return result;
        }
    }
}
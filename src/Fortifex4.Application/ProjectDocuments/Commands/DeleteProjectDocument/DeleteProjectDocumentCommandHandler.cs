using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Shared;
using Fortifex4.Shared.ProjectDocuments.Commands.DeleteProjectDocument;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.Application.ProjectDocuments.Commands.DeleteProjectDocument
{
    public class DeleteProjectDocumentCommandHandler : IRequestHandler<DeleteProjectDocumentRequest, DeleteProjectDocumentResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public DeleteProjectDocumentCommandHandler(IFortifex4DBContext context, IConfiguration configuration, IFileService fileService)
        {
            _context = context;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task<DeleteProjectDocumentResponse> Handle(DeleteProjectDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = new DeleteProjectDocumentResponse();
            var options = _configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>();

            var projectDocument = await _context.ProjectDocuments
                .Where(x => x.ProjectDocumentID == request.ProjectDocumentID)
                .SingleOrDefaultAsync(cancellationToken);

            if (projectDocument != null)
            {
                var folderPath = Path.Combine(options.ProjectDocumentsRootFolderPath, projectDocument.ProjectID.ToString());
                var fileName = $"{projectDocument.DocumentID}{Path.GetExtension(projectDocument.OriginalFileName)}";
                var removeFileResult = _fileService.RemoveFile(folderPath, fileName);

                result.IsSuccessful = true;
                result.ProjectID = projectDocument.ProjectID;

                _context.ProjectDocuments.Remove(projectDocument);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
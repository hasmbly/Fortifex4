using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Shared;
using Fortifex4.Shared.ProjectDocuments.Commands.UpdateProjectDocument;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.Application.ProjectDocuments.Commands.UpdateProjectDocument
{
    public class UpdateUploadFileProjectDocumentCommandHandler : IRequestHandler<UpdateProjectDocumentRequest, UpdateProjectDocumentResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public UpdateUploadFileProjectDocumentCommandHandler(IFortifex4DBContext context, IConfiguration configuration, IFileService fileService)
        {
            _context = context;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task<UpdateProjectDocumentResponse> Handle(UpdateProjectDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = new UpdateProjectDocumentResponse();
            var options = _configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>();

            var projectDocument = await _context.ProjectDocuments
                .Where(x => x.ProjectDocumentID == request.ProjectDocumentID)
                .SingleOrDefaultAsync(cancellationToken);

            if (projectDocument != null)
            {
                result.ProjectID = projectDocument.ProjectID;
                projectDocument.Title = request.Title;

                if (request.FormFileProjectDocument != null)
                {
                    var processFileResult = await _fileService.ProcessFile(request.FormFileProjectDocument);

                    if (processFileResult.IsSuccessful)
                    {
                        var originalFileName = WebUtility.HtmlEncode(request.FormFileProjectDocument.FileName);

                        var folderPath = Path.Combine(options.ProjectDocumentsRootFolderPath, projectDocument.ProjectID.ToString());
                        var trustedFileNameForFileStorage = $"{projectDocument.DocumentID}{Path.GetExtension(originalFileName)}";

                        var saveDocumentResult = await _fileService.SaveFile(processFileResult.FileContent, folderPath, trustedFileNameForFileStorage);

                        if (saveDocumentResult.IsSuccessful)
                        {
                            projectDocument.OriginalFileName = originalFileName;
                            await _context.SaveChangesAsync(cancellationToken);
                            result.IsSuccessful = true;
                        }
                        else
                        {
                            result.ErrorMessage = saveDocumentResult.ErrorMessage;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = processFileResult.ErrorMessage;
                    }
                }
                else
                {
                    await _context.SaveChangesAsync(cancellationToken);
                    result.IsSuccessful = true;
                }
            }

            return result;
        }
    }
}
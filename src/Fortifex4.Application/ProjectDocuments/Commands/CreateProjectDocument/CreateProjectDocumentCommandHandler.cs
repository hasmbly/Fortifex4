using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Domain.Entities;
using Fortifex4.Shared;
using Fortifex4.Shared.ProjectDocuments.Commands.CreateProjectDocument;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fortifex4.Application.ProjectDocuments.Commands.CreateProjectDocument
{
    public class CreateProjectDocumentCommandHandler : IRequestHandler<CreateProjectDocumentRequest, CreateProjectDocumentResponse>
    {
        private readonly IFortifex4DBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;

        public CreateProjectDocumentCommandHandler(IFortifex4DBContext context, IConfiguration configuration, IFileService fileService)
        {
            _context = context;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task<CreateProjectDocumentResponse> Handle(CreateProjectDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = new CreateProjectDocumentResponse();
            var originalFileName = WebUtility.HtmlEncode(request.FormFileProjectDocument.FileName);

            if (request.FormFileProjectDocument.Length > 0)
            {
                var options = _configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>();

                var existingProjectDocumentsCount = _context.ProjectDocuments
                    .Where(x => x.ProjectID == request.ProjectID).Count();

                if (existingProjectDocumentsCount < options.ProjectDocumentsLimit)
                {
                    var processFileResult = await _fileService.ProcessFile(request.FormFileProjectDocument);

                    if (processFileResult.IsSuccessful)
                    {
                        Guid documentID = Guid.NewGuid();

                        var projectDocument = new ProjectDocument
                        {
                            ProjectID = request.ProjectID,
                            Title = request.Title,
                            OriginalFileName = originalFileName,
                            DocumentID = documentID
                        };

                        _context.ProjectDocuments.Add(projectDocument);
                        await _context.SaveChangesAsync(cancellationToken);

                        var folderPath = Path.Combine(options.ProjectDocumentsRootFolderPath, request.ProjectID.ToString());
                        var trustedFileNameForFileStorage = $"{documentID}{Path.GetExtension(originalFileName)}";

                        var saveDocumentResult = await _fileService.SaveFile(processFileResult.FileContent, folderPath, trustedFileNameForFileStorage);

                        if (!saveDocumentResult.IsSuccessful)
                        {
                            result.ErrorMessage = saveDocumentResult.ErrorMessage;
                        }
                        else
                        {
                            result.IsSuccessful = true;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = processFileResult.ErrorMessage;
                    }
                }
                else
                {
                    result.ErrorMessage = $"Maximum file count ({options.ProjectDocumentsLimit}) has reached.";
                }
            }
            else
            {
                result.ErrorMessage = $"File {originalFileName} is empty.";
            }

            return result;
        }
    }
}
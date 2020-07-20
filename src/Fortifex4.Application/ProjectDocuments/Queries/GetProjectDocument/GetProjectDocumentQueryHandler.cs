using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Shared.ProjectDocuments.Queries.GetProjectDocument;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fortifex4.Application.ProjectDocuments.Queries.GetProjectDocument
{
    public class GetProjectDocumentHandler : IRequestHandler<GetProjectDocumentRequest, GetProjectDocumentResponse>
    {
        private readonly IFortifex4DBContext _context;

        public GetProjectDocumentHandler(IFortifex4DBContext context)
        {
            _context = context;
        }

        public async Task<GetProjectDocumentResponse> Handle(GetProjectDocumentRequest request, CancellationToken cancellationToken)
        {
            var result = new GetProjectDocumentResponse();

            var projectDocument = await _context.ProjectDocuments
                .Where(x => x.ProjectDocumentID == request.ProjectDocumentID)
                .SingleOrDefaultAsync(cancellationToken);

            if (projectDocument != null)
            {
                result.ProjectDocumentID = projectDocument.ProjectDocumentID;
                result.ProjectID = projectDocument.ProjectID;
                result.Title = projectDocument.Title;
                result.OriginalFileName = projectDocument.OriginalFileName;
                result.DocumentID = projectDocument.DocumentID;
            }

            return result;
        }
    }
}
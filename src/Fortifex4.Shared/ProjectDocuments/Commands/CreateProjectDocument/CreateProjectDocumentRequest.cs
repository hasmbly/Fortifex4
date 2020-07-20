using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.Shared.ProjectDocuments.Commands.CreateProjectDocument
{
    public class CreateProjectDocumentRequest : IRequest<CreateProjectDocumentResponse>
    {
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public IFormFile FormFileProjectDocument { get; set; }
    }
}
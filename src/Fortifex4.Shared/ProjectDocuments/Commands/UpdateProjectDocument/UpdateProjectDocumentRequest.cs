using MediatR;
using Microsoft.AspNetCore.Http;

namespace Fortifex4.Shared.ProjectDocuments.Commands.UpdateProjectDocument
{
    public class UpdateProjectDocumentRequest : IRequest<UpdateProjectDocumentResponse>
    {
        public int ProjectDocumentID { get; set; }
        public string Title { get; set; }
        public IFormFile FormFileProjectDocument { get; set; }
    }
}
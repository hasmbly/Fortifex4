using MediatR;

namespace Fortifex4.Shared.ProjectDocuments.Commands.DeleteProjectDocument
{
    public class DeleteProjectDocumentRequest : IRequest<DeleteProjectDocumentResponse>
    {
        public int ProjectDocumentID { get; set; }
    }
}
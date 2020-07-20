using MediatR;

namespace Fortifex4.Shared.ProjectDocuments.Queries.GetProjectDocument
{
    public class GetProjectDocumentRequest : IRequest<GetProjectDocumentResponse>
    {
        public int ProjectDocumentID { get; set; }
    }
}
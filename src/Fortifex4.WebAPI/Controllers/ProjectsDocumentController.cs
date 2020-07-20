using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Fortifex4.Shared.ProjectDocuments.Commands.CreateProjectDocument;
using Fortifex4.Shared.ProjectDocuments.Commands.DeleteProjectDocument;
using Fortifex4.Shared.ProjectDocuments.Commands.UpdateProjectDocument;
using Fortifex4.Shared.ProjectDocuments.Queries.GetProjectDocument;
using Fortifex4.WebAPI.Common;
using Fortifex4.WebAPI.Common.ApiEnvelopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Fortifex4.WebAPI.Controllers
{
    public class ProjectsDocumentController : ApiController
    {
        private readonly IFileProvider _fileProvider;

        public ProjectsDocumentController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        [AllowAnonymous]
        [HttpPost("createProjectDocument")]
        public async Task<ActionResult> CreateProjectDocument(CreateProjectDocumentRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProjectDocument/{projectDocumentID}")]
        public async Task<IActionResult> GetProjectDocument(int projectDocumentID)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(new GetProjectDocumentRequest() { ProjectDocumentID = projectDocumentID })));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpGet("getProjectDocument/download/{projectDocumentID}")]
        public async Task<IActionResult> GetProjectDocumentDownload(int projectDocumentID)
        {
            try
            {
                var result = await Mediator.Send(new GetProjectDocumentRequest() { ProjectDocumentID = projectDocumentID });

                var fileExtension = Path.GetExtension(result.OriginalFileName);

                var fileName = $"{result.DocumentID}{fileExtension}";

                var fileInfo = _fileProvider.GetFileInfo(Path.Combine(result.ProjectID.ToString(), fileName));
                string contentType = MimeTypeMap.GetMimeType(fileExtension);

                return Ok(new Success(result));
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPut("updateProjectDocument")]
        public async Task<IActionResult> UpdateProjectDocument(UpdateProjectDocumentRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }

        [Authorize]
        [HttpPost("deleteProjectDocument")]
        public async Task<IActionResult> DeleteProjectDocument(DeleteProjectDocumentRequest request)
        {
            try
            {
                return Ok(new Success(await Mediator.Send(request)));
            }
            catch (Exception exception)
            {
                return Ok(new InternalServerError(exception));
            }
        }
    }
}
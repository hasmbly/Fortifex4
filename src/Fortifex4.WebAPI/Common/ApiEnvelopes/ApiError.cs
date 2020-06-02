using System;
using System.Net;

namespace Fortifex4.WebAPI.Common.ApiEnvelopes
{
    public class ApiError : ApiEnvelope
    {
        public ApiError(int code, string defaultMessage)
        {
            this.Status = new Status()
            {
                IsError = true,
                Code = code,
                Message = defaultMessage
            };
        }

        public ApiError(int statusCode, string defaultMessage, object result)
            : this(statusCode, defaultMessage)
        {
            Result = result;
        }
    }

    public class InternalServerError : ApiError
    {
        public InternalServerError()
            : base(500, HttpStatusCode.InternalServerError.ToString())
        {
        }

        public InternalServerError(string message)
            : base(500, message)
        {
        }

        public InternalServerError(Exception exception)
            : base(500, exception.Message + ". StackTrace: " + exception.StackTrace)
        {
        }
    }

    public class NotFoundError : ApiError
    {
        public NotFoundError()
            : base(404, HttpStatusCode.NotFound.ToString())
        {
        }

        public NotFoundError(string message)
            : base(404, message)
        {
        }

        public NotFoundError(Exception exception)
            : base(404, exception.Message + ". StackTrace: " + exception.StackTrace)
        {
        }
    }

    public class BadRequestError : ApiError
    {
        public BadRequestError()
            : base(400, HttpStatusCode.BadRequest.ToString())
        {
        }

        public BadRequestError(string message)
            : base(400, message)
        {
        }
    }
}
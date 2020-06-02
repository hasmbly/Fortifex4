using System.Net;

namespace Fortifex4.WebAPI.Common.ApiEnvelopes
{
    public class ApiSuccess : ApiEnvelope
    {
        public ApiSuccess(int statusCode, string message)
        {
            this.Status = new Status() 
            {
                IsError = false,
                Code = statusCode,
                Message = message
            };
        }

        public ApiSuccess(int statusCode, string message, object result)
            : this(statusCode, message)
        {
            Result = result;
        }
    }

    public class Success : ApiSuccess
    {
        public Success()
            : base(200, HttpStatusCode.OK.ToString())
        {
        }

        public Success(object result)
            : base(200, HttpStatusCode.OK.ToString(), result)
        {
        }

        public Success(object result, string message)
            : base(200, message, result)
        {
        }
    }
}
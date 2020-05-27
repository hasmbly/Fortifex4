using System;
using System.Collections.Generic;

namespace Fortifex4.Application.Common.Models
{
    public class Result
    {
        internal Result(bool succeeded, IList<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors;
        }

        public bool Succeeded { get; set; }
        public IList<string> Errors { get; set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IList<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
using System.Collections.Generic;

namespace Application.Models
{
    public class Result
    {
        public Result() { }
        internal Result(bool succeeded, List<Error> errors = null)
        {
            Succeeded = succeeded;
            Errors = errors ;
        }

        public bool Succeeded { get; set; }
        public List<Error> Errors { get; set; }

        public static Result Success => new Result(true, new List<Error> { });

        public static Result Failed(List<Error> errors = null)
        {
            return new Result(false, errors);
        }
    }
}
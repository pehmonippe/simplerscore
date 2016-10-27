namespace SimplerScore.Exceptions
{
    using Attributes;
    using System;
    using System.Net;

    [HttpStatus(Status = HttpStatusCode.MethodNotAllowed)]
    public class ScoringModelAlreadyExistsException : Exception
    {
        public ScoringModelAlreadyExistsException ()
            : base("Cannot set new scoring model while previous exists.")
        {
        }
    }
}

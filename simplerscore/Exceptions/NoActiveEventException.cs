namespace SimplerScore.Exceptions
{
    using System;
    using System.Net;
    using Attributes;

    [HttpStatus(Status = HttpStatusCode.MethodNotAllowed)]
    public class NoActiveEventException : Exception
    {
        public NoActiveEventException ()
            : base("There is no active event selected.")
        {
        }
    }

    [HttpStatus(Status = HttpStatusCode.MethodNotAllowed)]
    public class NoActiveMeetException : Exception
    {
        public NoActiveMeetException ()
            : base("There is no active meet selected.")
        {
        }
    }
}
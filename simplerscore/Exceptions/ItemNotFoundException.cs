namespace SimplerScore.Exceptions
{
    using System;
    using System.Net;
    using Attributes;

    [HttpStatus(Status = HttpStatusCode.NotFound)]
    public class ItemNotFoundException : Exception
    {
    }
}

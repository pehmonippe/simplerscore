namespace SimplerScore.Exceptions
{
    using System;
    using Attributes;

    [HttpStatus(Status = System.Net.HttpStatusCode.NotFound)]
    public class NoCurrentException : Exception
    {
    }
}
namespace SimplerScore.Controllers
{
    using Attributes;
    using System;

    [HttpStatus(Status = System.Net.HttpStatusCode.NotFound)]
    internal class EntityNotFoundException : Exception
    {
    }
}

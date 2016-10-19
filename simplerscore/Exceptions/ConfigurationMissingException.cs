namespace SimplerScore.Exceptions
{
    using System;
    using Attributes;
    using JetBrains.Annotations;

    [HttpStatus(Status = System.Net.HttpStatusCode.NotAcceptable)]
    public class ConfigurationMissingException : Exception
    {
        public ConfigurationMissingException ([NotNull] string message)
            : base (message)
        {
        }
    }

    [HttpStatus(Status = System.Net.HttpStatusCode.BadRequest)]
    public class NullArgumentException : Exception
    {
        public NullArgumentException ([NotNull] string propertyName)
            : base($"Argument {propertyName} is null.")
        {
        }
    }
}

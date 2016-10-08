namespace SimplerScore.Attributes
{
    using System;
    using System.Net;

    /// <summary>
    /// Meta data attribute to map expection to suggested <see cref="HttpStatusCode"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class HttpStatusAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the suggested status code for exception.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public HttpStatusCode Status
        {
            get;
            set;
        }
    }
}
namespace SimplerScore.Controllers
{
    using DataAccess;
    using Exceptions;
    using System.Web.Http;

    public abstract class BaseController : ApiController
    {
        protected IDataProvider Provider
        {
            get;
        }

        protected BaseController (IDataProvider provider)
        {
            Provider = provider;
        }
    }
}

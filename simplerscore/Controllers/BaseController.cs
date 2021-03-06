namespace SimplerScore.Controllers
{
    using DataAccess;
    using Exceptions;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    public abstract class BaseController : ApiController
    {
        protected readonly IDataProvider Provider;

        protected BaseController ([NotNull] IDataProvider provider)
        {
            Provider = provider;
        }

        protected async Task<IEnumerable<T>> GetAll<T> ()
            where T : class, new()
        {
            var all = Provider.FindAll<T>();
            return await Task.FromResult(all);
        }

        protected async Task<T> Get<T> (int id)
            where T : class, new()
        {
            var e = Provider.Find<T>(id);
            return await Task.FromResult(e);
        }

        protected async Task<int> Add<T> ([NotNull] T e)
            where T : class, new()
        {
            if (null == e)
                throw new NullArgumentException(nameof(e));

            var id = Provider.Add(e);
            return await Task.FromResult(id);
        }

        protected async Task<int> Update<T> (int id, [NotNull] T e)
            where T : class, new()
        {
            if (null == e)
                throw new NullArgumentException(nameof(e));

            Provider.Update(e);

            return await Task.FromResult(id);
        }

        protected IHttpActionResult Delete<T> (int id)
            where T : class, new()
        {
            Provider.Delete<T>(id);
            return Ok();
        }
    }
}

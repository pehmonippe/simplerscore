namespace SimplerScore.Controllers
{
    using DataAccess;
    using DataObjects;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Exceptions;

    [RoutePrefix("event")]
    public class EventController : BaseController
    {
        public EventController (IDataProvider provider)
            : base(provider)
        {
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<Event> Get ([FromUri] int id)
        {
            var e = Provider.Find<Event>(id);
            return await Task.FromResult(e);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddEvent ([FromBody] Event e)
        {
            if (null == e)
                throw new NullArgumentException(nameof(e));

            var id = Provider.Add(e);
            return await Task.FromResult(id);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateEvent ([FromUri] int id, [FromBody] Event e)
        {
            if (null == e)
                throw new NullArgumentException(nameof(e));

            e.Id = id;
            Provider.Update(e);

            return await Task.FromResult(e.Id);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("{id:int}/delete")]
        public IHttpActionResult Delete ([FromUri] int id)
        {
            Provider.Delete<Event>(id);
            return Ok();
        }
    }
}

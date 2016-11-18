namespace SimplerScore.Controllers
{
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("event")]
    public class EventController : BaseController
    {
        public EventController ([NotNull] IDataProvider provider)
            : base(provider)
        {
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<Event> Get ([FromUri] int id)
        {
            return await Get<Event>(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddEvent ([FromBody] Event e)
        {
            return await Add(e);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateEvent ([FromUri] int id, [FromBody] Event e)
        {
            return await Update(id, e);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("{id:int}/delete")]
        public IHttpActionResult Delete ([FromUri] int id)
        {
            return Delete<Event>(id);
        }
    }

}

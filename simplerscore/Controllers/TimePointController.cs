namespace SimplerScore.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;
    using Models;

    [RoutePrefix("timepoint")]
    public class TimePointController : BaseController
    {
        public TimePointController ([NotNull] IDataProvider provider) 
            : base(provider)
        {
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<TimePoint> Get ([FromUri] int id)
        {
            return await Get<TimePoint>(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddTimePoint ([FromBody] TimePoint point)
        {
            return await Add(point);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateTimePoint ([FromUri] int id, [FromBody] TimePoint point)
        {
            return await Update(id, point);
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
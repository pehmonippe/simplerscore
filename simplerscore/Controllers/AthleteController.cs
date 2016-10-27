namespace SimplerScore.Controllers
{
    using DataObjects;
    using JetBrains.Annotations;
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("athlete")]
    public class AthleteController : BaseController
    {
        public AthleteController ([NotNull] IServiceProvider provider) 
            : base(provider)
        {
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<Athlete> Get ([FromUri] int id)
        {
            return await Get<Athlete>(id);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddAthlete ([FromBody] Athlete e)
        {
            return await Add(e);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateAthlete ([FromUri] int id, [FromBody] Athlete e)
        {
            e.Id = id;
            return await Update(id, e);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("{id:int}/delete")]
        public IHttpActionResult Delete ([FromUri] int id)
        {
            return Delete<Athlete>(id);
        }
    }
}

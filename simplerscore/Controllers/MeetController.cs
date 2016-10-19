namespace SimplerScore.Controllers
{
    using DataAccess;
    using DataObjects;
    using Exceptions;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("meet")]
    public class MeetController : BaseController
    {
        public MeetController (IDataProvider provider)
            : base(provider)
        {
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Meet>> Get ()
        {
            var all = Provider.FindAll<Meet>();
            return await Task.FromResult(all);
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddMeet ([FromBody] Meet meet)
        {
            if (null == meet)
                throw new NullArgumentException(nameof(meet));

            var id = Provider.Add(meet);
            return await Task.FromResult(id);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateMeet ([FromUri] int id, [FromBody] Meet meet)
        {
            if (null == meet)
                throw new NullArgumentException(nameof(meet));

            meet.Id = id;
            Provider.Update(meet);

            return await Task.FromResult(meet.Id);
        }

        //[Authorize(Roles="admin")]
        [HttpGet]
        [Route("{id:int}/delete")]
        public IHttpActionResult Delete ([FromUri] int id)
        {
            using (var transactionScope = Provider.BeginScope())
            {
                Expression<Func<Event, bool>> eventCriteria = e => e.MeetId == id;

                Provider.Delete(eventCriteria);
                Provider.Delete<Meet>(id);

                Provider.Commit(transactionScope);
            }
            
            return Ok();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<MeetModel> GetMeetDetails ([FromUri] int id)
        {
            var meet = Provider.Find<Meet>(id);

            Expression<Func<Event, bool>> eventCriteria = e => e.MeetId == id;

            var model = new MeetModel(meet)
            {
                Events = Provider.Find(eventCriteria).ConvertAll(e => new EventModel(e))
            };

            return await Task.FromResult(model);
        }
    }
}

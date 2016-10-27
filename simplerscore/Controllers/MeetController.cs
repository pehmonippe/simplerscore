namespace SimplerScore.Controllers
{
    using DataAccess;
    using DataObjects;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;
    using JetBrains.Annotations;
    using Models.Factories;

    [RoutePrefix("meet")]
    public class MeetController : BaseController
    {
        private readonly IModelFactoryContainer modelFactoryContainer;

        public MeetController ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer)
            : base(provider)
        {
            this.modelFactoryContainer = modelFactoryContainer;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Meet>> Get ()
        {
            return await GetAll<Meet>();
        }

        [HttpPost]
        [Route("")]
        public async Task<int> AddMeet ([FromBody] Meet meet)
        {
            return await Add(meet);
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<int> UpdateMeet ([FromUri] int id, [FromBody] Meet meet)
        {
            meet.Id = id;

            return await Update(id, meet);
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

            if (null == meet)
                return null;

            var model = (MeetModel) modelFactoryContainer.Create(meet, Provider);
            return await Task.FromResult(model);
        }
    }
}

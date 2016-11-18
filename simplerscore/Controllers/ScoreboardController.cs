namespace SimplerScore.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess;
    using DataObjects;
    using Extensions;
    using JetBrains.Annotations;
    using Models;
    using Models.Factories;

    [RoutePrefix("scoreboard")]
    public class ScoreboardController : BaseController
    {
        private readonly IModelFactoryContainer modelFactoryContainer;

        public ScoreboardController ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer) 
            : base(provider)
        {
            this.modelFactoryContainer = modelFactoryContainer;
        }

        [HttpGet]
        [Route("schedule/{id:int}")]
        public async Task<IEnumerable<ScheduleEntry>> GetSchedule ([FromUri] int id)
        {
            var meet = Provider.Find<Meet>(id);

            if (null == meet)
                return null;

            var model = (MeetModel)modelFactoryContainer.Create(meet, Provider);
            var schedule = model.ToScheduledOrder();

            return await Task.FromResult(schedule);
        }
    }
}

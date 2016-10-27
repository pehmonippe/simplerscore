namespace SimplerScore.Controllers
{
    using DataAccess;
    using JetBrains.Annotations;
    using System.Web.Http;

    public class CurrentController : BaseController
    {
        public CurrentController ([NotNull] IDataProvider provider) 
            : base(provider)
        {
        }

        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("next")]
        public IHttpActionResult MoveToNext ()
        {
            return Ok();
        }

        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("previous")]
        public IHttpActionResult MoveToPrevious ()
        {
            return Ok();
        }

        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("current")]
        public IHttpActionResult MoveTo ([FromUri] int eventId, [FromUri] int athleteId)
        {
            return Ok();
        }
    }
}

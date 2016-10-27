namespace SimplerScore.Controllers
{
    using System;
    using System.Web.Http;
    using JetBrains.Annotations;

    public class CurrentController : BaseController
    {
        public CurrentController ([NotNull] IServiceProvider provider) 
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

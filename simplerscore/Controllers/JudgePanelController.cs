namespace SimplerScore.Controllers
{
    using DataAccess;
    using JetBrains.Annotations;

    public class JudgePanelController : BaseController
    {
        public JudgePanelController ([NotNull] IDataProvider provider) : base(provider)
        {
        }
    }
}

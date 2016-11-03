namespace SimplerScore.Controllers
{
    using DataAccess;
    using JetBrains.Annotations;

    public class JudgesPanelController : BaseController
    {
        public JudgesPanelController ([NotNull] IDataProvider provider) 
            : base(provider)
        {
        }
    }
}

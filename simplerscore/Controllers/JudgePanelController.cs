namespace SimplerScore.Controllers
{
    using System;
    using JetBrains.Annotations;

    public class JudgePanelController : BaseController
    {
        public JudgePanelController ([NotNull] IServiceProvider provider) 
            : base(provider)
        {
        }
    }
}

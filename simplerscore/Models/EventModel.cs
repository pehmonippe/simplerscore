namespace SimplerScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Computation;
    using Extensions;
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;

    public class EventModel : Event
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDataProvider provider;

        private Lazy<IEnumerable<AthleteModel>> athletes;
        private Lazy<JudgePanelModel> judgePanel;

        public IEnumerable<AthleteModel> Athletes
        {
            get
            {
                var a = (athletes ?? (athletes = new Lazy<IEnumerable<AthleteModel>>(InitModelCollection)));
                return a.Value;
            }
        }

        public JudgePanelModel JudgePanel
        {
            get
            {
                var j = (judgePanel ?? (judgePanel = new Lazy<JudgePanelModel>(InitJudgePanel)));
                return j.Value;
            }
        }

        public EventModel ([NotNull] IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public EventModel ([NotNull] IServiceProvider serviceProvider, [NotNull] Event evnt)
            : this(serviceProvider)
        {
            Group = evnt.Group;
            Id = evnt.Id;
            MeetId = evnt.MeetId;
            Name = evnt.Name;
            Order = evnt.Order;
            Panel = evnt.Panel;
            ScoringStrategy = evnt.ScoringStrategy;
            ScheduleBehavior = evnt.ScheduleBehavior;
            ScheduledTime = evnt.ScheduledTime;
            Sponsor = evnt.Sponsor;

            provider = serviceProvider.GetService<IDataProvider>();
        }

        public IComputationStrategy GetComputationStrategy ()
        {
            var factory = serviceProvider.GetService<IComputationStrategyFactory>();
            var strategy = factory?.Create(ScoringStrategy);

            return strategy;
        }

        public int GetJudgeCount ()
        {
            //TODO: Get actual number from judgepanel....
            return 5;
        }

        private List<AthleteModel> InitModelCollection ()
        {
            if (null == provider)
                return new List<AthleteModel>();

            Expression<Func<Athlete, bool>> athleteCriteria = e => e.EventId == Id;

            var collection = provider.Collection<Athlete>()
                .Find(athleteCriteria)
                .ToList()
                .ConvertAll(athlete => new AthleteModel(athlete));

            return collection;
        }

        private JudgePanelModel InitJudgePanel ()
        {
            if (null == provider)
                return new JudgePanelModel();

            Expression<Func<JudgePanel, bool>> judgeCriteria = e => e.EventId == Id;

            var panel = provider.Collection<JudgePanel>().FindOne(judgeCriteria);
            
            if (null == panel)
                return new JudgePanelModel();

            var model = new JudgePanelModel(panel);
            return model;
        }
    }
}
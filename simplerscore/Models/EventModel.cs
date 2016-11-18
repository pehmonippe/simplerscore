namespace SimplerScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Computation;
    using DataAccess;
    using DataObjects;
    using Factories;
    using JetBrains.Annotations;

    public class EventModel : Event, IModel
    {
        private readonly IDataProvider provider;
        private readonly IComputationStrategyFactory strategyFactory;
        private readonly IModelFactoryContainer modelFactoryContainer;

        private Lazy<IEnumerable<AthleteModel>> athletes;
        private Lazy<JudgesPanelModel> judgePanel;

        public IEnumerable<AthleteModel> Athletes
        {
            get
            {
                var a = athletes ?? (athletes = new Lazy<IEnumerable<AthleteModel>>(InitModelCollection));
                return a.Value;
            }
        }

        public JudgesPanelModel JudgesPanel
        {
            get
            {
                var j = judgePanel ?? (judgePanel = new Lazy<JudgesPanelModel>(InitJudgePanel));
                return j.Value;
            }
        }

        public AthleteModel this[int athleteId]
        {
            get
            {
                var athlete = Athletes.FirstOrDefault(a => a.Id == athleteId);
                return athlete;
            }
        }

        public EventModel ([NotNull] IDataProvider provider, [NotNull] IComputationStrategyFactory strategyFactory, [NotNull] IModelFactoryContainer modelFactoryContainer)
        {
            this.provider = provider;
            this.strategyFactory = strategyFactory;
            this.modelFactoryContainer = modelFactoryContainer;
        }

        public EventModel ([NotNull] IDataProvider provider, [NotNull] IComputationStrategyFactory strategyFactory, [NotNull] Event evnt, [NotNull] IModelFactoryContainer modelFactoryContainer)
            : this(provider, strategyFactory, modelFactoryContainer)
        {
            //Group = evnt.Group;
            Id = evnt.Id;
            MeetId = evnt.MeetId;
            Name = evnt.Name;
            //Order = evnt.Order;
            Panel = evnt.Panel;
            ScoringStrategy = evnt.ScoringStrategy;
            //ScheduleBehavior = evnt.ScheduleBehavior;
            //ScheduledTime = evnt.ScheduledTime;
            Sponsor = evnt.Sponsor;

            this.provider = provider;
        }

        public IComputationStrategy GetComputationStrategy ()
        {
            var strategy = strategyFactory.Create(ScoringStrategy);
            return strategy;
        }

        public int GetJudgeCount ()
        {
            var nJudges = Panel?.ExecutionJudges?.Count;
            return nJudges ?? 5;
        }

        private List<AthleteModel> InitModelCollection ()
        {
            if (null == provider)
                return new List<AthleteModel>();

            Expression<Func<Athlete, bool>> athleteCriteria = e => e.EventId == Id;

            var factory = modelFactoryContainer.ModelFactoryOfType<Athlete>();

            var collection = provider.Collection<Athlete>()
                .Find(athleteCriteria)
                .ToList()
                .ConvertAll(athlete => (AthleteModel) factory.Create(athlete, provider, modelFactoryContainer));

            return collection;
        }

        private JudgesPanelModel InitJudgePanel ()
        {
            if (null == provider)
                return new JudgesPanelModel();

            Expression<Func<JudgesPanel, bool>> judgeCriteria = e => e.EventId == Id;

            var panel = provider.Collection<JudgesPanel>().FindOne(judgeCriteria);
            
            if (null == panel)
                return new JudgesPanelModel();

            var model = new JudgesPanelModel(panel);
            return model;
        }
    }
}
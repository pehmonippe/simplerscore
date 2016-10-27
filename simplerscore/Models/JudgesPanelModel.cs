namespace SimplerScore.Models
{
    using System.Linq;
    using DataObjects;
    using JetBrains.Annotations;

    public class JudgesPanelModel : JudgesPanel, IModel
    {
        public JudgesPanelModel ()
        {
        }

        public JudgesPanelModel ([NotNull] JudgesPanel judgesPanel)
        {
            Id = judgesPanel.Id;

            ChairOfJudgesPanel = judgesPanel.ChairOfJudgesPanel.Clone();
            ExecutionJudges = judgesPanel.ExecutionJudges.ToList();
            DifficultyJudge = judgesPanel.DifficultyJudge.Clone();
            TimeJudge = judgesPanel.TimeJudge.Clone();
        }
    }
}
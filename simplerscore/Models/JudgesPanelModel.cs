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
            DifficultyJudge = judgesPanel.DifficultyJudge.Clone();
            TimeJudge = judgesPanel.TimeJudge.Clone();

            ExecutionJudges = judgesPanel.ExecutionJudges
                .Select(ej => ej.Clone())
                .ToList();
        }
    }
}

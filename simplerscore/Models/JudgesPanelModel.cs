namespace SimplerScore.Models
{
    using DataObjects;
    using JetBrains.Annotations;

    public class JudgesPanelModel : JudgesPanel, IModel
    {
        public JudgesPanelModel ()
        {
        }

        public JudgesPanelModel ([NotNull] JudgesPanel judgesPanel)
        {
            ChairOfJudgesPanel = judgesPanel.ChairOfJudgesPanel;
            DifficultyJudge = judgesPanel.DifficultyJudge;
            FlightTimeJudge = judgesPanel.FlightTimeJudge;
            Id = judgesPanel.Id;
            Judge1 = judgesPanel.Judge1;
            Judge2 = judgesPanel.Judge2;
            Judge3 = judgesPanel.Judge3;
            Judge4 = judgesPanel.Judge4;
            Judge5 = judgesPanel.Judge5;
        }
    }
}
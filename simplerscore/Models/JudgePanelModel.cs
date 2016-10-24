namespace SimplerScore.Models
{
    using DataObjects;
    using JetBrains.Annotations;

    public class JudgePanelModel : JudgePanel
    {
        public JudgePanelModel ()
        {
        }

        public JudgePanelModel ([NotNull] JudgePanel judgePanel)
        {
            ChiefJudge = judgePanel.ChiefJudge;
            DifficultyJudge = judgePanel.DifficultyJudge;
            FlightTimeJudge = judgePanel.FlightTimeJudge;
            Id = judgePanel.Id;
            Judge1 = judgePanel.Judge1;
            Judge2 = judgePanel.Judge2;
            Judge3 = judgePanel.Judge3;
            Judge4 = judgePanel.Judge4;
            Judge5 = judgePanel.Judge5;
        }
    }
}
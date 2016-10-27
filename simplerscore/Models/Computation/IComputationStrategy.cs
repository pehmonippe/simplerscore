namespace SimplerScore.Models.Computation
{
    using System.Collections.Generic;
    using DataObjects;
    using JetBrains.Annotations;

    // See http://www.fig-gymnastics.com/publicdir/rules/files/tra/TRA-CoP%202013-2016%20(English).pdf
    // for code of points

    public interface IComputationStrategy
    {
        decimal ComputeScore ([NotNull, ItemNotNull] IEnumerable<Execution> executions, int time, int difficulty, int penalty);
    }
}
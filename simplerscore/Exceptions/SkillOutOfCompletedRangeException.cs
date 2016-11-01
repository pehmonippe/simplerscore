namespace SimplerScore.Exceptions
{
    using Attributes;
    using System;
    using System.Net;

    [HttpStatus(Status = HttpStatusCode.NotAcceptable)]
    public class SkillOutOfCompletedRangeException : Exception
    {
        public SkillOutOfCompletedRangeException (int skill, int completedElements)
            : base($"Skill ({skill}) exceeds the completed elements {completedElements}")
        {
        }
    }
}

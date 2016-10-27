namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;

    /// <summary>
    /// Execution represents all the possible deductions
    /// applied to a routine.
    /// </summary>
    public class Execution
    {
        /// <summary>
        /// Gets the skills. This is per element deduction.
        /// </summary>
        /// <value>
        /// The skills.
        /// </value>
        public List<int> Elements
        {
            get;
        }

        /// <summary>
        /// Gets or sets the landing. This is the landing deduction.
        /// </summary>
        /// <value>
        /// The landing.
        /// </value>
        public int Landing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the additional. This is additional deductions.
        /// </summary>
        /// <value>
        /// The additional.
        /// </value>
        public int Additional
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Execution" /> class.
        /// </summary>
        /// <param name="numberOfSkills">The number of skills.</param>
        public Execution (int numberOfSkills)
        {
            Elements = new List<int>(numberOfSkills);
        }
    }
}
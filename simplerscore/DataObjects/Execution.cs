namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using LiteDB;

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
            set;
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

        [BsonIgnore]
        public int Total
        {
            get
            {
                var total = Elements.Count * 10 - (Elements.Sum() + Landing + Additional);
                return total >= 0 ? total : 0;
            }
        }

        public Execution ()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Execution" /> class.
        /// </summary>
        /// <param name="numberOfSkills">The number of skills.</param>
        public Execution (int numberOfSkills)
        {
            Elements = new List<int>(numberOfSkills);

            for (var i = 0; i < numberOfSkills; i++)
                Elements.Add(0);
        }

        public Execution Clone ()
        {
            var clone = (Execution) MemberwiseClone();
            Elements.ForEach(e => clone.Elements.Add(e));

            return clone;
        }
    }
}
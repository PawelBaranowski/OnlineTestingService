using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// A complex object containing a question group and an integer number.
    /// It says how many questions are to be drawn from a group.
    /// </summary>
    [DisplayName("Number of questions")]
    public class GroupAndNumber : EntityWithId
    {
        /// <summary>
        /// Gets the question group.
        /// </summary>
        public virtual QuestionGroup Group { get; private set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        [Required]
        [DisplayName("Number of questions")]
        [DataType("GroupAndNumber")]
        [DefaultValue(0)]
        public virtual int Number { get; set; }

        /// <summary>
        /// Instantiates a new GroupAndNumber object.
        /// </summary>
        /// <param name="group">question group</param>
        /// <param name="number">number of questions</param>
        internal GroupAndNumber(QuestionGroup group, int number)
        {
            Group = group;
            Number = number;
        }

        public GroupAndNumber()
        {
            Group = new QuestionGroup("");
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((GroupAndNumber)obj).Group.Equals(this.Group);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Group.GetHashCode() + 19 * Number.GetHashCode();
        }
    }
}

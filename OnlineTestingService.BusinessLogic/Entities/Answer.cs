using System;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class hold candidates' answers.
    /// They also hold a numeric grade and a textual comment.
    /// The content of the answer cannot be changed, but grade and comment are modifiable.
    /// </summary>
    public class Answer
    {
        private int? grade = null;
        private string comment = "";
        private string content;

        /// <summary>
        /// Gets or sets a number indicating a grade this answer was given by the reviewer.
        /// It can be null (and is by default) what 
        /// </summary>
        virtual public int? Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        /// <summary>
        /// Gets or sets a comment given by the reviewer.
        /// It's an empty string by default.
        /// </summary>
        virtual public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        /// <summary>
        /// Gets or sets answer given by a candidate.
        /// </summary>
        virtual public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// Constructor with no arguments (for NHibernate).
        /// </summary>
        public Answer()
        {
            this.content = string.Empty;
            this.comment = string.Empty;
            this.grade = null;
        }

        /// <summary>
        /// Constructor with answer content.
        /// </summary>
        /// <param name="content">An answer given by candidate.</param>
        public Answer(string content)
        {
            this.content = content;
            this.comment = string.Empty;
            this.grade = null;
        }


    }
}

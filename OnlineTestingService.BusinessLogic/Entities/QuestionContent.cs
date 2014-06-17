using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class store a question and all its properties.
    /// </summary>
    public class QuestionContent : EntityWithId, EntityWithText
    {
        private IList<QuestionGroup> inGroups;

        /// <summary>
        /// Gets or sets whether the question is mandatory.
        /// </summary>
        virtual public bool Mandatory
        {
            get;
            set;
        }

        private string content;
        /// <summary>
        /// Gets textual content of the question.
        /// </summary>
        /// 
        [DataType("QuestionContent")]
        [Required]
        [RegularExpression(@"^[^ ]*.+[^ ]*$", ErrorMessage = "The content must be non-empty string and mustn't have any trailing or leadin spaces.")]
        virtual public String Content
        {
            get { return this.content; }
            set
            {
                if (this.content == null || this.content == string.Empty)
                {
                    this.content = value;
                }
            }
        }

        /// <summary>
        /// Gets number of minutes given by the question definer to answer it.
        /// </summary>
        [DataType("Time")]
        [Required]
        virtual public int Time
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not question is depricated.
        /// </summary>
        virtual public bool IsDepricated
        {
            get;
            set;
        }

        public virtual string NamesOfGroups
        {
            get
            {
                string names = string.Empty;
                foreach (QuestionGroup group in inGroups)
                {
                    if (names == string.Empty)
                    {
                        names += group.Name;
                    }
                    else
                    {
                        names += ", " + group.Name;
                    }
                }
                return names;
            }
            private set
            {
                string[] names = value.Split(',');
                foreach (string name in names)
                {
                    foreach (QuestionGroup group in Database.Instance.GetAllOfType<QuestionGroup>())
                    {
                        if (name.Equals(group.Name))
                        {
                            group.AddQuestion(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of groups the question belongs to.
        /// </summary>
        public virtual IList<QuestionGroup> InGroups
        {
            get
            {
                return new ReadOnlyCollection<QuestionGroup>(inGroups);
            }
            private set
            {
                inGroups = value;
            }
        }

        public QuestionContent()
        {
            this.inGroups = new List<QuestionGroup>();
            this.Time = 5;
        }

        /// <summary>
        /// Creates question with content, time and dutifulness.
        /// </summary>
        /// <param name="content">Question content.</param>
        /// <param name="minutes">Prescribed time in minutes for question.</param>
        /// <param name="mandatory">Do you want this question to be mandatory?</param>
        internal QuestionContent(string content, int minutes = 5, bool mandatory = false)
        {
            this.Content = content;
            this.Time = minutes;
            this.Mandatory = mandatory;
            this.Mandatory = false;
            inGroups = new List<QuestionGroup>();
        }

        /// <summary>
        /// Adds the question content to a question group.
        /// It should be called from QuestionGroup.AddQuestion!
        /// </summary>
        /// <param name="group">group to add the question content to</param>
        protected internal virtual void AddToGroup(QuestionGroup group)
        {
            if (!inGroups.Contains(group))
            {
                inGroups.Add(group);
            }
        }

        /// <summary>
        /// Removes the question content from a question group.
        /// It should be called from QuestionGroup.RemoveQuestion!
        /// </summary>
        /// <param name="group">group to remove the question content from</param>
        protected internal virtual void RemoveFromGroup(QuestionGroup group)
        {
            inGroups.Remove(group);
        }

        /// <summary>
        /// Returns true if the question content is not assigned to any group.
        /// </summary>
        public virtual bool IsNotInAnyGroup
        {
            get { return inGroups.Count == 0; }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((QuestionContent)obj).Content.Equals(this.Content);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Content.GetHashCode();
        }        
    }
}

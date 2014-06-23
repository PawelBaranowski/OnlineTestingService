using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class represent groups of questions.
    /// The group can contain any number of questions.
    /// A question may belong to one or more question groups.
    /// </summary>
    public class QuestionGroup : EntityWithId<QuestionGroup>, EntityWithText, CanDelete
    {
        public static readonly QuestionGroup NO_GROUP = new QuestionGroup { Name = "<not grouped>" };

        private IList<QuestionContent> questionContents;

        /// <summary>
        /// Gets or sets the name of the question group.
        /// </summary>
        [Required]
        [RegularExpression(@"^[^ ]*.+[^ ]*$", ErrorMessage = "The name must be non-empty string and mustn't have any trailing or leadin spaces.")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets a list of all questions that belong to this group.
        /// </summary>
        public virtual IList<QuestionContent> QuestionContents {
            get { return new ReadOnlyCollection<QuestionContent>(questionContents); }
            //get { return questionContents; }
            private set { questionContents = value; }
        }

        /// <summary>
        /// Instantiates a new QuestionGroup.
        /// </summary>
        /// <param name="name">name of the new group</param>
        internal QuestionGroup(string name)
        {
            Name = name;
            QuestionContents = new List<QuestionContent>();
        }

        public QuestionGroup()
        {
        }

        /// <summary>
        /// Adds a new question to the group.
        /// </summary>
        /// <param name="questionContent">a question to add</param>
        /// <param name="cascade">if true, the changes are propagated to the question content</param>
        public virtual void AddQuestion(QuestionContent questionContent, bool cascade = true)
        {
            if (!QuestionContents.Contains(questionContent))
            {
                questionContents.Add(questionContent);
                if (cascade)
                {
                    questionContent.AddToGroup(this);
                }
            }
        }

        /// <summary>
        /// Removes a question from the group.
        /// </summary>
        /// <param name="questionContent">a question to remove</param>
        /// <param name="cascade">if true, the changes are propagated to the question content</param>
        public virtual void RemoveQuestion(QuestionContent questionContent, bool cascade = true)
        {
            questionContents.Remove(questionContent);
            if (cascade)
            {
                questionContent.RemoveFromGroup(this);
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is QuestionGroup))
            {
                return false;
            }
            return ((QuestionGroup)obj).Name.Equals(this.Name);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

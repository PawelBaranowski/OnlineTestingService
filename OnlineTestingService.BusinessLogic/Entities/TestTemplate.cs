using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// A definition of a test.
    /// </summary>
    public class TestTemplate : EntityWithId<TestTemplate>, EntityWithText
    {
        private const char SEPARATOR = ';';

        /// <summary>
        /// Gets the name of the template.
        /// The name must be unique! It cannot be changed.
        /// </summary>
        [Required]
        [RegularExpression(@"^[^ ]*.+[^ ]*$", ErrorMessage = "The name must be non-empty string and mustn't have any trailing or leadin spaces.")]
        public virtual string Name { get; set; }

        public virtual bool IsDeprecated { get; set; }

        /// <summary>
        /// Gets or sets a file containing job offer for which this test templated is created.
        /// </summary>
        public virtual File JobOffer { get; set; }

        public virtual IList<GroupAndNumber> GroupsAndNumbers { get; private set; }

        public virtual int NumberOfQuestions
        {
            get
            {
                int num = 0;
                foreach (var gan in GroupsAndNumbers)
                {
                    num += gan.Number;
                }
                return num;
            }
        }

        /// <summary>
        /// Gets a dictionary whose keys are question groups in the template and values say how many questions are to be picked from each group.
        /// </summary>
        public virtual IDictionary<QuestionGroup, int> QuestionGroups
        {
            get
            {
                Dictionary<QuestionGroup, int> dict = new Dictionary<QuestionGroup, int>();
                foreach (GroupAndNumber item in GroupsAndNumbers)
                {
                    dict[item.Group] = item.Number;
                }
                return dict;
            }
            private set
            {
                List<GroupAndNumber> result = new List<GroupAndNumber>();
                foreach (var item in value)
                {
                    result.Add(new GroupAndNumber(item.Key, item.Value));
                }
                GroupsAndNumbers = result;
            }
        }

        /// <summary>
        /// Gets or sets a string with e-mail addresses separated with SEPARATOR.
        /// </summary>
        protected virtual string Emails { get; private set; }

        /// <summary>
        /// Gets a list of e-mails to send notifications to when a test created with this template is finished.
        /// </summary>
        public virtual IList<EmailAddress> PeopleToNotify
        {
            get
            {
                IList<EmailAddress> result = new List<EmailAddress>();
                if (string.IsNullOrEmpty(Emails))
                {
                    return result;
                }
                foreach(string s in Emails.Split(SEPARATOR))
                {
                    result.Add(new EmailAddress(s));
                }
                return new ReadOnlyCollection<EmailAddress>(result);
            }
            private set
            {
                IList<string> strings = new List<string>();
                foreach (EmailAddress e in value)
                {
                    strings.Add(e.Address);
                }
                Emails = string.Join(SEPARATOR.ToString(), strings);
            }
        }

        /// <summary>
        /// Creates a new test template with the given name.
        /// </summary>
        /// <param name="name"></param>
        internal TestTemplate(string name)
        {
            IsDeprecated = false;
            Name = name;
            Emails = string.Empty;
            GroupsAndNumbers = new List<GroupAndNumber>();
        }
       
        public TestTemplate()
        {
            GroupsAndNumbers = new List<GroupAndNumber>();
        }

        /// <summary>
        /// Adds a new question group or changes the number of questions for a previously added group.
        /// </summary>
        /// <param name="questionGroup">guestion group to add/update</param>
        /// <param name="howManyQuestions">number of questions to set</param>
        public virtual void AddOrUpdateQuestionGroup(QuestionGroup questionGroup, int howManyQuestions)
        {
            //if (howManyQuestions == 0)
            //{
            //    return;
            //}
            if (howManyQuestions > questionGroup.QuestionContents.Count)
            {
                howManyQuestions = questionGroup.QuestionContents.Count;
            }            
            GroupAndNumber gan = new GroupAndNumber(questionGroup, howManyQuestions);
            GroupsAndNumbers.Remove(gan);
            GroupsAndNumbers.Add(gan);
        }
        
        /// <summary>
        /// Removes a guestion group.
        /// </summary>
        /// <param name="questionGroup">question group to remove</param>
        public virtual void RemoveQuestionGroup(QuestionGroup questionGroup)
        {
            GroupAndNumber gan = new GroupAndNumber(questionGroup, 1);
            GroupsAndNumbers.Remove(gan);
        }
        
        /// <summary>
        /// Adds a new e-mail address to the list of notification receivers for this template.
        /// </summary>
        /// <param name="email">e-mail address to add</param>
        protected virtual void AddEmailAddress(EmailAddress email)
        {
            if (Emails.IndexOf(email.Address) < 0)
            {
                if (Emails.Length > 0) Emails += SEPARATOR;
                Emails += email.Address;
            }
        }

        /// <summary>
        /// Adds an e-mail address using its string value.
        /// </summary>
        /// <param name="address">address string</param>
        public virtual void AddEmailAddress(string address)
        {
            AddEmailAddress(new EmailAddress(address));
        }

        /// <summary>
        /// Removes an e-mail address from the list of notification receivers for this template.
        /// </summary>
        /// <param name="email">e-mail address to remove</param>
        protected virtual void RemoveEmailAddress(EmailAddress email)
        {
            int startIndex = Emails.IndexOf(email.Address);
            if (startIndex < 0)
            {
                return;
            }
            int endIndex = Emails.IndexOf(SEPARATOR, startIndex);
            // if this is the last address it doesn't have SEPARATOR at the end!
            if (endIndex < 0)
            {
                endIndex = Emails.Length - 1;
            }
            Emails = Emails.Remove(startIndex, endIndex - startIndex + 1);
            // we must ensure there isn't a single SEPARATOR at the end!
            if (Emails.Length > 0 && Emails[Emails.Length - 1].Equals(SEPARATOR))
            {
                Emails = Emails.Remove(Emails.Length - 1);
            }
        }

        /// <summary>
        /// Removes an e-mail address using its string value.
        /// </summary>
        /// <param name="address"></param>
        public virtual void RemoveEmailAddress(string address)
        {
            RemoveEmailAddress(new EmailAddress(address));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((TestTemplate)obj).Name.Equals(this.Name);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

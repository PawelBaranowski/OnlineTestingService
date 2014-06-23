using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineTestingService.BusinessLogic.Entities
{
    public class Candidate : EntityWithId<Candidate>, EntityWithText
    {
        virtual public File CV { get; set; }
        virtual public EmailAddress EmailAddress { get; set; }
        [Required]
        [RegularExpression(@"^[^ ]*.+[^ ]*$", ErrorMessage = "The name must be non-empty string and mustn't have only spaces.")]
        virtual public string Name { get; set; }
        virtual public PhoneNumber PhoneNumber { get; set; }
        virtual public bool Inactive { get; set; }

        public virtual User User { get; set; }
        public virtual IList<Skill> PerfectSkills { get; set; }
        public virtual IList<Skill> GoodSkills { get; set; }
        public virtual IList<Skill> BasicSkills { get; set; }

        public Candidate()
        {
            Name = string.Empty;
            PerfectSkills = new List<Skill>();
            GoodSkills = new List<Skill>();
            BasicSkills = new List<Skill>();
        }
        internal Candidate(string name, PhoneNumber phoneNumber = null, EmailAddress emailAddress = null, File cv = null) : this()
        {
            Name = name;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            CV = cv;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Candidate other = (Candidate)obj;
            return other.EmailAddress.Equals(this.EmailAddress) && other.Name.Equals(this.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + 17 * EmailAddress.GetHashCode();
        }
    }
}

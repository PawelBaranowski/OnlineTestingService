using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class hold all information about a canndidate: name, e-mail address, telephone number and a CV file.
    /// </summary>
    public class Candidate : EntityWithId, EntityWithText
    {
        /// <summary>
        /// Gets or sets a file containing the candidate's CV.
        /// </summary>
        virtual public File CV { get; set; }

        /// <summary>
        /// Gets or sets the candidate's e-mail address.
        /// </summary>
        virtual public EmailAddress EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the candidate's name.
        /// </summary>
        [Required]
        [RegularExpression(@"^[^ ]*.+[^ ]*$", ErrorMessage = "The name must be non-empty string and mustn't have only spaces.")]
        virtual public string Name { get; set; }

        /// <summary>
        /// Gets or sets the candidate's phone number.
        /// </summary>
        virtual public PhoneNumber PhoneNumber { get; set; }

        virtual public bool Inactive { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Candidate()
        {
            Name = string.Empty;
            Inactive = false;
        }

        /// <summary>
        /// Creates complete Candidate.
        /// </summary>
        /// <param name="name">Name of candidate.</param>
        /// <param name="phoneNumber">Candidates phone number.</param>
        /// <param name="emailAddress">Candidates email address.</param>
        /// <param name="cv">Candidates CV.</param>
        internal Candidate(string name, PhoneNumber phoneNumber = null, EmailAddress emailAddress = null, File cv = null)
        {
            Inactive = false;
            Name = name;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            CV = cv;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Candidate other = (Candidate)obj;
            return other.EmailAddress.Equals(this.EmailAddress) && other.Name.Equals(this.Name);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Name.GetHashCode() + 17 * EmailAddress.GetHashCode();
        }
    }
}

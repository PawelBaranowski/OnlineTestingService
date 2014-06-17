using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// This class provides a convenient way of storing and validating e-mail addresses.
    /// As a new object is created or 'Address' property is set the address string is validated using a regular expression.
    /// </summary>
    [DisplayName("E-mail address")]
    public class EmailAddress
    {
        private const string regexp = @"^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        private string address;

        /// <summary>
        /// Gets or sets a string containing a valid e-mail address.
        /// </summary>
        [Required]
        [DisplayName("E-mail address")]
        [RegularExpression(regexp, ErrorMessage="Please enter a vali e-mail address!")]
        virtual public string Address
        {
            get { return address; }
            set {
                if (validate(value, true)) address = value.Trim();
                else throw new Exception("Validation failed. Email Address creation failed.");
            }
            
        }

        /// <summary>
        /// Checks is <code>emailAddress</code> validates and creates new EmailAddress if so.
        /// Throws exception if invalidate.
        /// </summary>
        /// <param name="emailAddress">address string</param>
        public EmailAddress(string emailAddress)
        {
            Address = emailAddress;
        }

        /// <summary>
        /// Initializes empty EmailAddress.
        /// </summary>
        public EmailAddress()
        {
            address = string.Empty;
        }

        /// <summary>
        /// This method checks if the given string is a valid e-mail address.
        /// </summary>
        /// <param name="address">address string</param>
        /// <param name="strong">bool defining if validation should be strong (set to true if so, but it tooks longer. 
        /// The default value is false.</param>
        /// <returns><code>true</code> if <code>address</code> is a valid e-mail address, <code>false</code> otherwise</returns>
        public static bool validate(string address, bool strong = false)
        {
            bool isValidate = false;
            if (strong)
            {
                string patternStrict = regexp;
                Regex reStrict = new Regex(patternStrict);
                //check if address matches the pattern.
                isValidate = reStrict.IsMatch(address.Trim());
            }
            else
            {
                string patternLenient = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                Regex reLenient = new Regex(patternLenient);
                //check if address matches the pattern.
                isValidate = reLenient.IsMatch(address.Trim());
            }

            return isValidate;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((EmailAddress)obj).Address.Equals(this.Address);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            if (Address != null) return Address.GetHashCode();
            else return 0;
        }
    }
}

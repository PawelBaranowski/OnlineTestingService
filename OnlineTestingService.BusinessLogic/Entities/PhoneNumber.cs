using System;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// This class provides a convenient way of storing and validating phone numbers.
    /// As a new object is created or 'Number' property is set the phone number string is validated using a regular expression.
    /// </summary>
    public class PhoneNumber
    {
        private const string regexp = @"^(\+[0-9]{2})?([0-9]{9})|(([0-9]{3}-){2}[0-9]{3})$";

        private string number;

        /// <summary>
        /// Gets a string containing a valid phone number.
        /// </summary>
        [DisplayName("Phone number")]
        [RegularExpression(regexp, ErrorMessage="Please enter a valid phone number!")]
        virtual public string Number
        {
            get { return this.number; }
            set { if (validate(value)) this.number = value.Trim(); }
        }

        /// <summary>
        /// Creates new PhoneNumber with specified number but only if it validates.
        /// If not, throws standard exception.
        /// </summary>
        /// <param name="number">Phone number</param>
        public PhoneNumber(string number)
        {
            if (validate(number))
            {
                this.number = number.Trim();
            }
            else
            {
                throw new Exception("Phone number is not valid.");
            }
        }

        /// <summary>
        /// Initializes empty phone number.
        /// </summary>
        public PhoneNumber()
        {
        }

        /// <summary>
        /// This method checks if the given string is a valid phone number.
        /// It accepts for example "123-456-789" and "123456789".
        /// </summary>
        /// <param name="address">phone number string</param>
        /// <returns><code>true</code> if <code>number</code> is a valid phone number, <code>false</code> otherwise</returns>
        public static bool validate(string number)
        {
            if (number == null) return false;
            bool isValidate = false;

            //this pattern will match 123-456-789 and 123456789, 
            //which is pretty much what we need.
            string pattern = regexp;
            
            Regex reg = new Regex(pattern);
            //check if string matches pattern
            isValidate = reg.IsMatch(number.Trim());

            return isValidate;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return ((PhoneNumber)obj).Number.Equals(this.Number);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            if (Number != null) return Number.GetHashCode();
            else return 0;
        }
    }
}

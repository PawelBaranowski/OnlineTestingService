using System;
using OnlineTestingService.BusinessLogic.Entities;
using System.Text;

namespace OnlineTestingService.BusinessLogic
{
    /// <summary>
    /// This class holds two notification templates and creates personalized notifications for candidates and employees.
    /// It is a singleton class.
    /// </summary>
    internal class Notification
    {
        private static string DELIM = "$";
        private static string NEW_LINE = @"\n";
        private static Notification instance;

        /// <summary>
        /// Sets the e-mail text template used to inform a candidate about a new test.
        /// </summary>
        public string CandidateNotificationTemplate { private get; set; }
        /// <summary>
        /// Sets the e-mail text template used to inform an employee about a finished test.
        /// </summary>
        public string EmployeeNotificationTemplate { private get; set; }

        /// <summary>
        /// Creates a notification text informing a candidate that a new test was created for them.
        /// </summary>
        /// <param name="test">a <see cref="Test"/> to notify about</param>
        /// <returns></returns>
        public string GetNotificationForCandidate(Test test)
        {
            if (CandidateNotificationTemplate == null)
            {
                throw new Exception("Candidate notification template was not properly set!");
            }

            return FillFieldsWithProperties(CandidateNotificationTemplate, test);
        }

        /// <summary>
        /// Creates a notification text informing an employee that a test was finished.
        /// </summary>
        /// <param name="test">a <see cref="Test"/> to notify about</param>
        /// <returns></returns>
        public string GetNotificationForEmployee(Test test)
        {
            if (EmployeeNotificationTemplate == null)
            {
                throw new Exception("Employee notification template was not properly set!");
            }
            return FillFieldsWithProperties(EmployeeNotificationTemplate, test);
        }

        /// <summary>
        /// The only instance of <code>Notification</code> class.
        /// </summary>
        public static Notification Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Notification();
                }
                return instance;
            }
        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private Notification()
        {
            CandidateNotificationTemplate = null;
            EmployeeNotificationTemplate = null;
        }

        public string FillFieldsWithProperties(string template, object obj)
        {
            StringBuilder result = new StringBuilder(template);
            result.Replace(NEW_LINE, Environment.NewLine);

            int dollarPos = template.IndexOf(DELIM);
            int nextDollar;
            while (dollarPos > -1)
            {
                nextDollar = template.IndexOf(DELIM, dollarPos + 1);
                string propertyName = template.Substring(dollarPos + 1, nextDollar - dollarPos - 1);
                System.Reflection.PropertyInfo property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    string propertyValue = property.GetValue(obj, null).ToString();
                    result.Replace(DELIM + propertyName + DELIM, propertyValue);
                }
                dollarPos = template.IndexOf(DELIM, nextDollar + 1);
            }
            return result.ToString();
        }
    }
}

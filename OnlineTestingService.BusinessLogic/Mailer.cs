using System;
using System.Collections.Generic;
using OnlineTestingService.BusinessLogic.Entities;
using System.Net.Mail;
using System.IO;

namespace OnlineTestingService.BusinessLogic
{
    /// <summary>
    /// This class is responsible for sending e-mail notifictions to candidates and company's employees.
    /// It's a singleton class.
    /// Notification about finished tests are sent to employee's in batches as often as the system administrator decides.
    /// </summary>
    public class Mailer
    {
        private static Mailer instance = null;
        private Notification notification;
        private List<Test> finishedTests;

        /// <summary>
        /// Gets the only instance of <code>Mailer</code> class.
        /// </summary>
        public static Mailer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Mailer(Notification.Instance);
                }
                return instance;
            }
        }

        /// <summary>
        /// Sets an SMPT client to use to send e-mails.
        /// </summary>
        public SmtpClient SmptClient { private get; set; }

        /// <summary>
        /// Sets the 'from' address for all e-mails that will be sent.
        /// </summary>
        public string FromAddress { private get; set; }

        /// <summary>
        /// Gets or sets the subject to use for e-mails that inform a candidate about a new test.
        /// </summary>
        public string CandidateNotificationSubject { private get; set; }

        /// <summary>
        /// Gets or sets the subject to use for e-mails that inform an employee about a finished test.
        /// </summary>
        public string EmployeeNotificationSubject { private get; set; }

        /// <summary>
        /// Sets the e-mail text template used to inform a candidate about a new test.
        /// </summary>
        public string CandidateNotificationTemplate
        {
            set
            {
                notification.CandidateNotificationTemplate = value;
            }
        }
        /// <summary>
        /// Sets the e-mail text template used to inform an employee about a finished test.
        /// </summary>
        public string EmployeeNotificationTemplate
        {
            set
            {
                notification.EmployeeNotificationTemplate = value;
            }
        }

        public string UrlPrefix
        {
            private get;
            set;
        }
    
        /// <summary>
        /// Adds a new finished test to notify employee's about in the next batch.
        /// </summary>
        /// <param name="test">a <see cref="Test"/> to notify about in the next batch</param>
        public void AddFinishedTest(Test test)
        {
            finishedTests.Add(test);
        }

        /// <summary>
        /// Immediatly sends a notification to a candidate that a new test was created for them.
        /// </summary>
        /// <param name="test">a <see cref="Test"/> to notify about</param>
        internal void NotifyCandidate(Test test, string password)
        {
            string messageBody = notification.GetNotificationForCandidate(test);
            messageBody = notification.FillFieldsWithProperties(messageBody, new { Password = password, TestUrl = UrlPrefix + "/Test/" + test.Guid });
            MailMessage message = new MailMessage(FromAddress, test.Candidate.EmailAddress.Address, CandidateNotificationSubject, messageBody);
            //using (StreamWriter output = new StreamWriter(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\mail-output.txt"))
            //{
            //    output.Write(messageBody);
            //}
            SmptClient.Send(message);
        }

        /// <summary>
        /// Sends notifications about finished tests for all tests previously added with <see cref="AddFinishedTest(Test)"/> method.
        /// </summary>
        public void NotifyEmployees()
        {            
            foreach (Test test in finishedTests)
            {
                string messageBody = notification.GetNotificationForEmployee(test);
                MailMessage message = new MailMessage();
                message.From = new MailAddress(FromAddress);
                foreach (EmailAddress address in test.TestTemplate.PeopleToNotify)
                {
                    message.To.Add(address.Address);
                }
                message.Subject = EmployeeNotificationSubject;
                message.Body = messageBody;
                //SmptClient.Send(message);
                
            }
        }

        /// <summary>
        /// Sends a custom notification.
        /// The notification template must have property names of the given object enclosed in dollar signs.
        /// If the object has a property calles 'Name' the template should have a line like:
        /// <code>Hello $Name$!</code>
        /// To break a line use '\n' (explicitly! a backslash and 'n' letter).
        /// If <code>obj == null</code>, nothing is substituted in the template.
        /// </summary>
        /// <param name="email">email address to send the notification to</param>
        /// <param name="subject">subject of the message</param>
        /// <param name="notificationTemplate">template to fill with property values</param>
        /// <param name="obj">object which holds the properties</param>
        public void SendCustomNotification(EmailAddress email, string subject, string notificationTemplate, object obj)
        {
            string messageBody;
            if (obj != null)
            {
                messageBody = notification.FillFieldsWithProperties(notificationTemplate, obj);
            }
            else
            {
                messageBody = notificationTemplate;
            }
            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromAddress);
            message.To.Add(email.Address);
            message.Subject = subject;
            message.Body = messageBody;
            SmptClient.Send(message);
        }

        /// <summary>
        /// A private constructor.
        /// </summary>
        /// <param name="notification">class <see cref="Notification"/> object</param>
        private Mailer(Notification notification)
        {
            this.notification = notification;
            this.finishedTests = new List<Test>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class represent a concrete test with all <see cref="Question"/>s and candidate's info.
    /// </summary>
    public class Test : EntityWithGuid
    {
        public enum TestStatus
        {
            NOT_FINISHED = 0,
            NOT_REVIEWED,
            REVIEWED
        }

        /// <summary>
        /// A list of all questions in the test.
        /// </summary>
        private IList<Question> questions;

        /// <summary>
        /// Gets start time of the test.
        /// </summary>
        virtual public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets end time of the test.
        /// </summary>
        virtual public DateTime EndTime { get; private set; }

        /// <summary>
        /// Gets a list of all questions in the test.
        /// </summary>
        virtual public IList<Question> Questions
        {
            get { return new ReadOnlyCollection<Question>(questions); }
            private set { questions = value; }
        }

        /// <summary>
        /// Gets test template this test was created with.
        /// </summary>
        virtual public TestTemplate TestTemplate { get; private set; }

        ///// <summary>
        ///// Gets the tests GUID.
        ///// </summary>
        //virtual public string GUID { get; private set; }

        /// <summary>
        /// Gets the password needed to access the test.
        /// </summary>
        virtual public string PasswordHash { get; private set; }

        /// <summary>
        /// Gets the candidate this test was created for.
        /// </summary>
        virtual public Candidate Candidate { get; private set; }

        virtual public string CandidateName { get { return Candidate.Name; } }
        virtual public string CandidateEmailAddress { get { return Candidate.EmailAddress.Address; } }
        virtual public string CandidatePhoneNumber { get { return Candidate.PhoneNumber.Number; } }

        /// <summary>
        /// Says whether or not test is finished (by candidate).
        /// </summary>
        virtual public bool IsFinished { get; private set; }

        /// <summary>
        /// Gets or sets whether the test was reviewed or not.
        /// </summary>
        virtual public bool IsReviewed { get; set; }

        virtual public TestStatus Status
        {
            get
            {
                if (!IsFinished) return TestStatus.NOT_FINISHED;
                else
                {
                    if (!IsReviewed) return TestStatus.NOT_REVIEWED;
                    else return TestStatus.REVIEWED;
                }
            }
        }

        /// <summary>
        /// Gets the duration time for the test (in minutes).
        /// </summary>
        virtual public int Duration { get; private set; }

        /// <summary>
        /// Checks if the time given for answering all questions has passed.
        /// </summary>
        /// <returns></returns>
        virtual public bool HasExpired()
        {
            if (IsFinished)
            {
                return true;
            }
            if (DateTime.Now.CompareTo(EndTime) >= 0)
            {
                Finish();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Starts the test. Sets start and end times.
        /// </summary>
        virtual public void Start()
        {
            if (this.StartTime.Equals(new DateTime(1900, 01, 01)))
            {
                StartTime = DateTime.Now;
                EndTime = StartTime;
                EndTime = EndTime.AddMinutes(Duration);
            }
        }

        /// <summary>
        /// Finishes the test.
        /// </summary>
        virtual public void Finish()
        {
            EndTime = DateTime.Now;
            IsFinished = true;
            Mailer.Instance.AddFinishedTest(this);
        }

        virtual public void FinishReview()
        {
            if (!IsFinished)
            {
                throw new InvalidOperationException("Cannot finish review on an unfinished test!");
            }
            else
            {
                IsReviewed = true;
            }
        }

        virtual public int Grade
        {
            get
            {
                int grade = 0;
                foreach (Question question in Questions)
                {
                    if (question.Answer.Grade == null) return -1;
                    grade += question.Answer.Grade.Value;
                }
                if (Questions.Count == 0) return 0;
                else return grade / Questions.Count;
            }

        }

        /// <summary>
        /// Initializes empty Test.
        /// </summary>
        protected Test()
        {
        }

        /// <summary>
        /// Initializes complete Test.
        /// </summary>
        /// <param name="candidate">Candidate assigned to the test.</param>
        /// <param name="questions">List of questions for the test.</param>
        /// <param name="password">Password for the Test.</param>
        /// <param name="guid">GUID of the Test.</param>
        internal Test(TestTemplate testTemplate, Candidate candidate, List<Question> questions, string password, string guid)
        {
            TestTemplate = testTemplate;
            Candidate = candidate;
            Questions = questions;
            PasswordHash = password;
            //GUID = guid;
            Duration = 0;
            //Sets the duration based on questions:
            foreach (Question question in Questions)
            {
                Duration += question.QuestionContent.Time;
            }
            //To make sure the test is valid.
            //Not to get out of bounds of SQL Server type
            StartTime = new DateTime(1900, 01, 01);
            EndTime = DateTime.MaxValue.AddDays(-1.0);
            IsFinished = false;
            IsReviewed = false;
        }
    }
}

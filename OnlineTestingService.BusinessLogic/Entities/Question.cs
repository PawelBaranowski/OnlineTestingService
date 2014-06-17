using System;

namespace OnlineTestingService.BusinessLogic.Entities
{
    /// <summary>
    /// Objects of this class represent a unique question that holds an answer.
    /// </summary>
    public class Question : EntityWithId
    {
        private QuestionContent questionContent;
        private Answer answer;

        /// <summary>
        /// Gets content of this question.
        /// </summary>
        virtual public QuestionContent QuestionContent
        {
            get { return this.questionContent; }
            private set { this.questionContent = value; }
        }

        /// <summary>
        /// Gets <see cref="Answer"/> given by a candidate.
        /// </summary>
        virtual public Answer Answer
        {
            get { return this.answer; }
            set { this.answer = value; }
        }

        protected Question()
        {
        }

        /// <summary>
        /// Creates Question with specified question content.
        /// </summary>
        /// <param name="questionContent">Question content for this question.</param>
        internal Question(QuestionContent questionContent)
        {
            this.questionContent = questionContent;
            this.answer = new Answer();
        }
    }
}

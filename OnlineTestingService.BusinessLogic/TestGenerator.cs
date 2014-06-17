using System;
using OnlineTestingService.BusinessLogic.Entities;
using System.Collections.Generic;

namespace OnlineTestingService.BusinessLogic
{
    internal struct TestWithPassword
    {
        internal Test Test { get; set; }
        internal string Password { get; set; }
    }

    /// <summary>
    /// This class generates a new test.
    /// It's a singleton class.
    /// </summary>
    public class TestGenerator
    {
        /// <summary>
        /// Generates a new test.
        /// </summary>
        /// <param name="template">template to generate test with</param>
        /// <param name="candidate">candidate to generate test for</param>
        /// <returns></returns>
        internal static TestWithPassword GenerateTest(TestTemplate template, Candidate candidate)
        {
            List<Question> questions = new List<Question>();
            foreach (GroupAndNumber item in template.GroupsAndNumbers)
            {
                List<QuestionContent> mandatoryQuestions = new List<QuestionContent>();
                List<QuestionContent> nonMandatoryQuestions = new List<QuestionContent>();
                foreach (QuestionContent content in item.Group.QuestionContents)
                {
                    if (content.Mandatory)
                    {
                        mandatoryQuestions.Add(content);
                    }
                    else
                    {
                        nonMandatoryQuestions.Add(content);
                    }
                }
                int howMany = item.Number;
                while (howMany > 0 && mandatoryQuestions.Count > 0)
                {
                    int whichOne = randomizer.Next(mandatoryQuestions.Count - 1);
                    questions.Add(new Question(mandatoryQuestions[whichOne]));
                    mandatoryQuestions.RemoveAt(whichOne);
                    howMany--;
                }
                while (howMany > 0 && nonMandatoryQuestions.Count > 0)
                {
                    int whichOne = randomizer.Next(nonMandatoryQuestions.Count - 1);
                    questions.Add(new Question(nonMandatoryQuestions[whichOne]));
                    nonMandatoryQuestions.RemoveAt(whichOne);
                    howMany--;
                }
                if (howMany > 0)
                {
                    throw new Exception("There where not enough questions in the question group!");
                }
            }

            string password = PasswordGenerator.MakePassword();
            string passwordHash = PasswordGenerator.encodeToMD5(password);

            Test test = new Test(template, candidate, questions, passwordHash, Guid.NewGuid().ToString());

            return new TestWithPassword { Test = test, Password = password };
        }

        private static Random randomizer = new Random((int)DateTime.Now.Ticks);
    }
}

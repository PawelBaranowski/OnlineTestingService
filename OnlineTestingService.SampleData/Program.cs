using OnlineTestingService.BusinessLogic;
using OnlineTestingService.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTestingService.SampleData
{
    class Program
    {
        private static Database Instance;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", @"C:\Data\OnlineTestingService\");
            Database.Setup();
            Instance = Database.Instance;
            FillWithSampleData();
        }

        private static void FillWithSampleData()
        {
            QuestionContent question;

            #region Questions in group 'Java'
            QuestionGroup java = Instance.MakeNew<QuestionGroup>("Java");

            question = Instance.MakeNew<QuestionContent>(
                  "What gets printed when the following code is compiled and run with command:\n\n"
                + "java test 2\n\n"
                + "public class test {\n"
                + "    public static void main(String args[]) {\n"
                + "        Integer intObj=Integer.valueOf(args[args.length-1]);\n"
                + "        int i = intMbj.intValue();\n"
                + "        if(args.length > 1)\n"
                + "            System.out.println(i);\n"
                + "        if(args.length > 0)\n"
                + "            System.out.println(i - 1);\n"
                + "        else\n"
                + "            System.out.println(i - 2);\n"
                + "    }"
                + "}");
            question.Time = 3;
            question.Mandatory = true;
            java.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "What does the \"abstract\" keyword mean in front of a method declaration?\n"
                + "What does it mean in front of a class declaration?"
                );
            question.Time = 2;
            question.Mandatory = true;
            java.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "How to add two integer two numbers using Bitwise operators?\n"
                );
            question.Time = 5;
            question.Mandatory = true;
            java.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "How would you perform a transaction using JDBC?\n"
                );
            question.Time = 3;
            question.Mandatory = false;
            java.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "Is it possible to serialize a static field?\n"
                );
            question.Time = 2;
            question.Mandatory = false;
            java.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "Can we add a Hashtable/HashMap to a vector?\n"
                );
            question.Time = 3;
            question.Mandatory = false;
            java.AddQuestion(question);
            Database.Instance.Save(java);
            #endregion
            #region Questions in group '.NET'
            QuestionGroup net = Instance.MakeNew<QuestionGroup>(".NET");

            question = Instance.MakeNew<QuestionContent>(
                  "What is a constructor in C#?"
                );
            question.Time = 2;
            question.Mandatory = true;
            net.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "What does Dispose() method do?"
                );
            question.Time = 2;
            question.Mandatory = true;
            net.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "If a namespace is not supplies what namespace does a class belong to?"
                );
            question.Time = 1;
            question.Mandatory = true;
            net.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "What are delegates?"
                );
            question.Time = 3;
            question.Mandatory = false;
            net.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "How do you use DBCC statements to monitor various Aspects of a SQL Server installation?"
                );
            question.Time = 3;
            question.Mandatory = false;
            net.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
                  "How would you create a thread in C#?"
                );
            question.Time = 3;
            question.Mandatory = false;
            net.AddQuestion(question);
            Database.Instance.Save(net);
            #endregion
            #region Questions in group 'C\C++'
            QuestionGroup cpp = Instance.MakeNew<QuestionGroup>(@"C\C++");

            question = Instance.MakeNew<QuestionContent>(
            "Can a file other than a .h file be included with #include in a C program?"
            );
            question.Time = 1;
            question.Mandatory = true;
            cpp.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
              "Write in C or C++  the equivalent expression for:\n"
            + "x%8"
            );
            question.Time = 5;
            question.Mandatory = true;
            cpp.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
              "Anything wrong with this code?\n"
            + "T *p = new T[10]; delete p;"
            );
            question.Time = 5;
            question.Mandatory = true;
            cpp.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What are the 2 ways of exporting a function from a DLL?"
            );
            question.Time = 2;
            question.Mandatory = false;
            cpp.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What’s the auto keyword good for?"
            );
            question.Time = 3;
            question.Mandatory = false;
            cpp.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How do you decide which integer type to use?"
            );
            question.Time = 2;
            question.Mandatory = false;
            cpp.AddQuestion(question);
            Database.Instance.Save(cpp);
            #endregion
            #region Questions in group 'Web design'
            QuestionGroup web = Instance.MakeNew<QuestionGroup>("Web design");

            question = Instance.MakeNew<QuestionContent>(
            "Describe PHP error and logging information."
            );
            question.Time = 3;
            question.Mandatory = true;
            web.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How do you align a table to the right (or left) using CSS2?"
            );
            question.Time = 2;
            question.Mandatory = true;
            web.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How do you center block-elements with CSS1?"
            );
            question.Time = 3;
            question.Mandatory = true;
            web.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How to embed a JavaScript file in a web page? "
            );
            question.Time = 2;
            question.Mandatory = false;
            web.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How to force a page to go to another location using JavaScript?"
            );
            question.Time = 2;
            question.Mandatory = false;
            web.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What are XML indexes and secondary XML indexes?"
            );
            question.Time = 3;
            question.Mandatory = false;
            web.AddQuestion(question);
            Database.Instance.Save(web);
            #endregion
            #region Questions in group 'Testing'
            QuestionGroup testing = Instance.MakeNew<QuestionGroup>("Testing");

            question = Instance.MakeNew<QuestionContent>(
            "What is elapsed time?"
            );
            question.Time = 2;
            question.Mandatory = true;
            testing.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What automating testing tools are you familiar with?"
            );
            question.Time = 3;
            question.Mandatory = true;
            testing.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What is a 'test plan'?"
            );
            question.Time = 3;
            question.Mandatory = true;
            testing.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How can World Wide Web sites be tested?"
            );
            question.Time = 3;
            question.Mandatory = false;
            testing.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What is the QTP testing process?"
            );
            question.Time = 2;
            question.Mandatory = false;
            testing.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What is Extreme Programming and what's it got to do with testing?"
            );
            question.Time = 4;
            question.Mandatory = false;
            testing.AddQuestion(question);
            Database.Instance.Save(testing);
            #endregion
            #region Questions in group 'Database management'
            QuestionGroup dbmng = Instance.MakeNew<QuestionGroup>("Database management");

            question = Instance.MakeNew<QuestionContent>(
            "How To Delete a Login Name in MS SQL Server?"
            );
            question.Time = 3;
            question.Mandatory = true;
            dbmng.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "How to display an n-th highest record in a table using MYSQL?"
            );
            question.Time = 5;
            question.Mandatory = true;
            dbmng.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "Describe the use of %ROWTYPE and %TYPE in PL/SQL"
            );
            question.Time = 5;
            question.Mandatory = true;
            dbmng.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What are some good ideas regarding user security in MySQL?"
            );
            question.Time = 3;
            question.Mandatory = false;
            dbmng.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What is a view? How it is related to data independence?"
            );
            question.Time = 4;
            question.Mandatory = false;
            dbmng.AddQuestion(question);

            question = Instance.MakeNew<QuestionContent>(
            "What is serialization?"
            );
            question.Time = 4;
            question.Mandatory = false;
            dbmng.AddQuestion(question);
            Database.Instance.Save(dbmng);
            #endregion
            #region Templates management
            var testTemplate = Instance.MakeNew<TestTemplate>("Java");
            testTemplate.AddEmailAddress("marysia@kowalska.pl");
            testTemplate.AddEmailAddress("john@borczyk.com");
            testTemplate.AddEmailAddress("pawel@baranowski.pl");
            testTemplate.RemoveEmailAddress("john@borczyk.com");
            testTemplate.RemoveEmailAddress("marysia@kowalska.pl");
            testTemplate.AddEmailAddress("kamila@jeszczekaminska.pl");

            testTemplate.AddOrUpdateQuestionGroup(java, 4);
            testTemplate.AddOrUpdateQuestionGroup(cpp, 4);

            Instance.Save(testTemplate);
            #endregion
            #region Candidates
            var candidate1 = Instance.MakeNew<Candidate>("Gabriel");
            candidate1.EmailAddress = (new EmailAddress("gabrybass@gmail.com"));
            Instance.Save(candidate1);
            var candidate2 = Instance.MakeNew<Candidate>("User2");
            candidate2.EmailAddress = (new EmailAddress("fakemail@gmail.com"));
            candidate2.PhoneNumber = new PhoneNumber("+48742212123");
            Instance.Save(candidate2);
            #endregion
            #region Tests
            var test1 = Instance.MakeNew<Test>(testTemplate, candidate1, false);
            var test2 = Instance.MakeNew<Test>(testTemplate, candidate2, false);
            test2.Finish();
            int i = 0;
            foreach (Question q in test2.Questions) //Grade all questions in the test
            {
                q.Answer.Grade = 5;
                if (i > 4) q.Answer.Grade = 2;
                i++;
            }
            test2.FinishReview();
            Instance.Save(test2);
            #endregion
            #region Skills
            var skills = new[] { "C#", "Java", "C++", "PHP", "JavaScript", "Node", "Ruby on rails" };
            foreach (var skill in skills)
            {
                Database.Instance.MakeNew<Skill>(skill);
            }
            #endregion
        }
    }
}

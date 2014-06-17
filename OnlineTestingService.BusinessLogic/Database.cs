using System;
using OnlineTestingService.BusinessLogic.Entities;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace OnlineTestingService.BusinessLogic
{
    /// <summary>
    /// Provides an interface for creating and manipulating persistent objects.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Gets the only instance of <c>Database</c> class.
        /// </summary>
        public static Database Instance
        {
            get
            {
                if (instance == null)
                    instance = new Database();
                return instance;
            }
            private set { instance = value; }
        }

        #region Methods that create new business objects
        /// <summary>
        /// Creates new objects that can be constructed using one string parameter.
        /// </summary>
        /// <typeparam name="T">type of the object to create</typeparam>
        /// <param name="nameOrContent">string object to use</param>
        /// <returns>a persistent object representing an entity</returns>
        public T MakeNew<T>(string nameOrContent) where T : EntityWithText
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                Type type = typeof(T);
                EntityWithText entity;
                if (type.Equals(typeof(Candidate)))
                    entity = new Candidate(nameOrContent);
                else if (type.Equals(typeof(QuestionContent)))
                    entity = new QuestionContent(nameOrContent);
                else if (type.Equals(typeof(QuestionGroup)))
                    entity = new QuestionGroup(nameOrContent);
                else if (type.Equals(typeof(TestTemplate)))
                    entity = new TestTemplate(nameOrContent);
                else
                    throw new NotImplementedException("Creation of '" + type.FullName + "' objects is not supported!\nPerhaps its a new feature that needs to be implemented?");
                session.Save(entity);
                transaction.Commit();
                return (T)entity;
            }
        }
        /// <summary>
        /// Stores a file in the database and returns a persistent object for further use.
        /// </summary>
        /// <typeparam name="T">must always be <c>File</c></typeparam>
        /// <param name="file">a file from HTTP request to use</param>
        /// <returns>a persistent file representation for further use</returns>
        public File MakeNew<T>(System.Web.HttpPostedFileBase file) where T : File
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                File aFile = new File(file);
                session.Save(aFile);
                transaction.Commit();
                return aFile;
            }
        }
        /// <summary>
        /// Creates a new <c>Test</c> and returns a persistent object.
        /// </summary>
        /// <typeparam name="T">must always be <c>Test</c></typeparam>
        /// <param name="testTemplate">test template to use</param>
        /// <param name="candidate">candidate to create the test for</param>
        /// <returns>a persistent representing the test</returns>
        public Test MakeNew<T>(TestTemplate testTemplate, Candidate candidate) where T : Test
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                TestWithPassword testWithPassword = TestGenerator.GenerateTest(testTemplate, candidate);
                session.Save(testWithPassword.Test);
                transaction.Commit();
                Mailer.Instance.NotifyCandidate(testWithPassword.Test, testWithPassword.Password);
                return testWithPassword.Test;
            }
        }
        #endregion

        #region Methods that manipulate existing business objects
        /// <summary>
        /// Persists changes made in a business object.
        /// </summary>
        /// <param name="businessObject">business object whose state needs to be persisted</param>
        public void Save(Entity businessObject)
        {
            using (var transaction = getSaveSession.BeginTransaction())
            {
                getSaveSession.Update(businessObject);
                transaction.Commit();
            }
        }
        /// <summary>
        /// Deletes a persistent object.
        /// </summary>
        /// <param name="businessObject">the object to delete</param>
        public void Delete(CanDelete businessObject)
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(businessObject);
                transaction.Commit();
            }
        }
        /// <summary>
        /// Gets a list of all persistent objects of a given type.
        /// </summary>
        /// <typeparam name="T">type of objects to find</typeparam>
        /// <returns>a list of all persistent objects of type T</returns>
        public IList<T> GetAllOfType<T>() where T : Entity
        {
            return getSaveSession.CreateCriteria<T>().List<T>();
        }

        public IList<T> GetAllOfType<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            return (IList<T>)getSaveSession.QueryOver<T>().Where(expression).List();
        }

        /// <summary>
        /// Gets an object of a given type with the specified id.
        /// Returns null if there is no such an object.
        /// </summary>
        /// <typeparam name="T">type of object to get</typeparam>
        /// <param name="id">object's id</param>
        /// <returns>the persistent object or null</returns>
        public T GetById<T>(int id) where T : EntityWithId
        {
            return getSaveSession.Get<T>(id);
        }
        /// <summary>
        /// Gets an object of a given type with the specified Guid.
        /// Returns null if there is no such an object.
        /// </summary>
        /// <typeparam name="T">type of object to get</typeparam>
        /// <param name="guid">object's Guid</param>
        /// <returns>the persistent object or null</returns>
        public T GetByGuid<T>(Guid guid) where T : EntityWithGuid
        {
            return getSaveSession.Get<T>(guid);
        }
        #endregion

        #region Methods that manipulate the database
        /// <summary>
        /// Cleans up and recreates the database.
        /// Must be called before first read from Instance property.
        /// Use with extreme care!
        /// NOTE : Perhaps it sould be password-protected?
        /// </summary>
        public static void Setup(string connectionString, bool reset = false, bool populateSampleData = false)
        {
            if (instance == null)
            {
                instance = new Database(reset);
                if (populateSampleData)
                    FillWithSampleData();
            }
        }



        #endregion

        #region Private section

        public static string DbFile { get { return AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\database.db"; } }
        private static Database instance;
        private ISessionFactory sessionFactory;
        private ISession getSaveSession;

        /// <summary>
        /// Instantiates a new <c>Database</c> object.
        /// <param name="reset">if it is true, the database will be cleaned-up</param>
        /// </summary>
        private Database(bool reset = false)
        {
            sessionFactory = CreateSessionFactory(reset);
            getSaveSession = sessionFactory.OpenSession();
        }

        /// <summary>
        /// Destroys the object. Closes active session and session factory.
        /// </summary>
        ~Database()
        {
            getSaveSession.Close();
            sessionFactory.Close();
        }

        /// <summary>
        /// Creates a new session factory and configures NHibernate.
        /// </summary>
        /// <returns></returns>
        private static ISessionFactory CreateSessionFactory(bool reset = false)
        {
            string connectionString =
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
            if (reset)
            {
                return Fluently.Configure()
                      .Database(
                        MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)
                      )
                      .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<Database>())
                      .ExposeConfiguration(BuildSchema)
                      .BuildSessionFactory();
            }
            else
            {
                return Fluently.Configure()
                      .Database(
                        MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)
                      )
                      .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<Database>())
                      .BuildSessionFactory();
            }
        }

        /// <summary>
        /// Injects relational schema in the database. Use with extreme care!
        /// </summary>
        /// <param name="config">NHibernate configuration to use</param>
        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            if (System.IO.File.Exists(DbFile))
                System.IO.File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
              .Create(false, true);
        }

        /// <summary>
        /// Fills the database with some sample data.
        /// </summary>
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
                var test1 = Instance.MakeNew<Test>(testTemplate, candidate1);
                var test2 = Instance.MakeNew<Test>(testTemplate, candidate2);
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

        }
        #endregion

        

    }
}

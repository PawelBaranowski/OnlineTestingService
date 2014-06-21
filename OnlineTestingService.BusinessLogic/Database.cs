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
        public Test MakeNew<T>(TestTemplate testTemplate, Candidate candidate, bool notify = true) where T : Test
        {
            using (var session = sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                TestWithPassword testWithPassword = TestGenerator.GenerateTest(testTemplate, candidate);
                session.Save(testWithPassword.Test);
                transaction.Commit();
                if (notify) Mailer.Instance.NotifyCandidate(testWithPassword.Test, testWithPassword.Password);
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
        public static void Setup()
        {
            if (instance == null)
            {
                instance = new Database();
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
        private Database()
        {
            sessionFactory = CreateSessionFactory();
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
        private static ISessionFactory CreateSessionFactory()
        {
            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
            return Fluently.Configure()
                      .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                      .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Database>())
                      .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                      .BuildSessionFactory();
        }
        #endregion

        

    }
}

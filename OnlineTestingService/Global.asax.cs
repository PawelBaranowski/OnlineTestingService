using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Mail;
using OnlineTestingService.BusinessLogic;
using System.Xml.Serialization;
using System.IO;
using OnlineTestingService.BusinessLogic.Entities;
using System.Data.SqlClient;
using System.Threading;

namespace OnlineTestingService
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Test",
                "Test/{guid}",
                new { controller = "Test_", action = "Index", guid = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Tests", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Setup();
        }

        protected void Setup()
        {
            Mailer mailer = Mailer.Instance;
            mailer.SmptClient = new SmtpClient();
            mailer.FromAddress = Settings.SendEmailFromAddress;
            mailer.CandidateNotificationSubject = Settings.CandidateNotificationSubject;
            mailer.CandidateNotificationTemplate = Settings.CandidateNotificationTemplate;
            mailer.EmployeeNotificationSubject = Settings.EmployeeNotificationSubject;
            mailer.EmployeeNotificationTemplate = Settings.EmployeeNotificationTemplate;
            mailer.UrlPrefix = Settings.UrlPrefix;
            MailerDaemon.Start();

            string connectionString =
                System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                
                connection.Open();
                connection.Close();
                Database.Setup(connectionString);
            }
            catch (SqlException e)
            {
                //Some time-out is needed here
                //Probably the 'connection' object doesn't get disposed on time
                connection.Dispose();
                Thread.Sleep(1000);

                Models.UserManagement userMngmnt = Models.UserManagement.GetInstance();
                System.Web.Security.MembershipCreateStatus status = new System.Web.Security.MembershipCreateStatus();
                userMngmnt.Setup();
                Models.User user;

                user = new Models.User(
                    "admin", "online-testing-service@o2.pl", "admin", new string[] {}, System.Guid.NewGuid());
                user.IsAdmin = true;
                userMngmnt.AddUser(user, out status);

                user = new Models.User(
                    "candidateM", "online-testing-service@o2.pl", "candidateM", new string[] {}, System.Guid.NewGuid());
                user.IsCandidateManager = true;
                userMngmnt.AddUser(user, out status);

                user = new Models.User(
                    "testD", "online-testing-service@o2.pl", "testD", new string[] {}, System.Guid.NewGuid());
                user.IsTestDefiner = true;
                userMngmnt.AddUser(user, out status);

                user = new Models.User(
                    "testR", "online-testing-service@o2.pl", "testR", new string[] {}, System.Guid.NewGuid());
                user.IsTestReviewer = true;
                userMngmnt.AddUser(user, out status);
                
                Database.Setup(connectionString, true, true);
            }
        }
    }
}
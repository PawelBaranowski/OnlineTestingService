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

            Database.Setup();
        }
    }
}
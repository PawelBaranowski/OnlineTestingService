using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using OnlineTestingService.BusinessLogic;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService
{
    public class MailerDaemon
    {
        private static Thread thread;

        public static void Start()
        {
            thread = new Thread(new ThreadStart(SendAndWait));
            thread.Start();
        }

        public static void Stop()
        {
            thread.Abort();
        }

        public static void SendInstantly()
        {
            Mailer.Instance.NotifyEmployees();
        }

        private static void SendAndWait()
        {
            while (true)
            {
                SendInstantly();
                Thread.Sleep(Settings.NotificationDelay);
            }
        }
    }
}
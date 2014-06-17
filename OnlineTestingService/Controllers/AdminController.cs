using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.SqlClient;
using OnlineTestingService.Models;
using OnlineTestingService.BusinessLogic;

namespace OnlineTestingService.Controllers
{
    [Authorize(Roles=Models.User.ADMIN)]
    public class AdminController : Controller
    {
        /// <summary>
        /// Main action for this page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(UserManagement.GetInstance().GetAllUsers());
        }

        /// <summary>
        /// Action for Select command (when radgrid needs data to bind).
        /// </summary>
        /// <returns></returns>
        public ActionResult Select()
        {
            return View("Index", UserManagement.GetInstance().GetAllUsers());
        }

        /// <summary>
        /// Action for inserting new user into database.
        /// </summary>
        /// <returns></returns>
        public ActionResult Insert(User insertedUser)
        {
            //TODO: read status and set proper message to ViewBag.

            Models.User myUser = new Models.User();
            myUser.UserID = System.Guid.NewGuid();
            //myUser.Password = "hardCodedPassword";
            if (TryUpdateModel(myUser))
            {
                MembershipCreateStatus status = new MembershipCreateStatus();
                UserManagement.GetInstance().AddUser(myUser, out status);                                
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Action for deleting user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        /// [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string username)
        {
            //TO DO
            //check if delete was successul, set ViewBag message.
            bool deleted = UserManagement.GetInstance().DeleteUser(username);

            return RedirectToAction("Index");
        }

        public ActionResult Update(string username)
        {
            User myUser = new User();
            if (TryUpdateModel(myUser))
            {
                UserManagement.GetInstance().UpdateUser(myUser);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TimeSelected(string TimesList)
        {
            //TO DO
            //Set time in mailer.
            TimesList.Trim();
            int delay = int.Parse(TimesList);
            Settings.NotificationDelay = delay;
            MailerDaemon.Stop();
            MailerDaemon.Start();
            return RedirectToAction("Index");
        }
    }
}

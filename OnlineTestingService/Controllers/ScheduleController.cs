using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic.Entities;
using OnlineTestingService.BusinessLogic;
using System.Reflection;

namespace OnlineTestingService.Controllers
{
    [Authorize(Roles=Models.User.CANDIDATE_MANAGER + "," + Models.User.ADMIN)]
    public class ScheduleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

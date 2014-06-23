using OnlineTestingService.BusinessLogic;
using OnlineTestingService.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineTestingService.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Database Database { get { return Database.Instance; } }

        protected User CurrentUser
        {
            get
            {
                var userId = (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey;
                return Database.Instance.GetByGuid<OnlineTestingService.BusinessLogic.Entities.User>(userId);
            }
        }
        protected Candidate CurrentCandidate
        {
            get
            {
                return CurrentUser.Candidate;
            }
        }
    }
}
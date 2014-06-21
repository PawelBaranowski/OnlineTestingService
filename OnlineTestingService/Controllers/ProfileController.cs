using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic.Entities;
using OnlineTestingService.BusinessLogic;
using System.Reflection;
using OnlineTestingService.Models;

namespace OnlineTestingService.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        public ActionResult Index(int? id)
        {
            Candidate candidate;
            if (id == null)
            {
                var user = GetUser();
                candidate = user.Candidate;
            }
            else
            {
                candidate = Database.Instance.GetById<Candidate>(id.Value);
            }

            var model = new CandidateProfileViewModel();
            model.PerfectSkills = candidate.PerfectSkills.Select(x => x.Id).ToArray();
            model.GoodSkills = candidate.GoodSkills.Select(x => x.Id).ToArray();
            model.BasicSkills = candidate.BasicSkills.Select(x => x.Id).ToArray();
            return View(model);
        }

        public ActionResult Skills()
        {
            var skills = Database.Instance.GetAllOfType<Skill>();

            return Json(skills, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProfile(CandidateProfileViewModel model)
        {
            var user = GetUser();
            var candidate = user.Candidate;
            var skills = Database.Instance.GetAllOfType<Skill>();

            candidate.PerfectSkills = skills.Where(skill => model.PerfectSkills.Contains(skill.Id)).ToList();
            candidate.GoodSkills = skills.Where(skill => model.GoodSkills.Contains(skill.Id)).ToList();
            candidate.BasicSkills = skills.Where(skill => model.BasicSkills.Contains(skill.Id)).ToList();
            Database.Instance.Save(candidate);

            return RedirectToAction("Index");
        }

        private OnlineTestingService.BusinessLogic.Entities.User GetUser()
        {
            var userId = (Guid)System.Web.Security.Membership.GetUser().ProviderUserKey;
            return Database.Instance.GetByGuid<OnlineTestingService.BusinessLogic.Entities.User>(userId);
        }
    }
}

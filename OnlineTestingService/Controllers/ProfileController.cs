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
    public class ProfileController : BaseController
    {
        public ActionResult Index(int? id)
        {
            Candidate candidate;
            if (id == null)
            {
                candidate = CurrentCandidate;
            }
            else
            {
                candidate = Database.GetById<Candidate>(id.Value);
            }

            var model = new CandidateProfileViewModel();
            model.PerfectSkills = candidate.PerfectSkills.Select(x => x.Id).ToArray();
            model.GoodSkills = candidate.GoodSkills.Select(x => x.Id).ToArray();
            model.BasicSkills = candidate.BasicSkills.Select(x => x.Id).ToArray();
            return View(model);
        }

        public ActionResult Skills()
        {
            var skills = Database.GetAllOfType<Skill>();

            return Json(skills, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveProfile(CandidateProfileViewModel model)
        {
            var candidate = CurrentCandidate;
            var skills = Database.GetAllOfType<Skill>();

            candidate.PerfectSkills = skills.Where(skill => model.PerfectSkills.Contains(skill.Id)).ToList();
            candidate.GoodSkills = skills.Where(skill => model.GoodSkills.Contains(skill.Id)).ToList();
            candidate.BasicSkills = skills.Where(skill => model.BasicSkills.Contains(skill.Id)).ToList();
            Database.Save(candidate);

            return RedirectToAction("Index");
        }
    }
}

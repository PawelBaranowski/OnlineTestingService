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
    public class CandidatesController : Controller
    {
        //
        // GET: /Candidates/

        public ActionResult Index()
        {
            return View(Database.Instance.GetAllOfType<Candidate>(c => c.Inactive == false));
        }

        public ActionResult SelectCandidate(int? id)
        {
            if (id != null)
            {
                ViewData["id"] = (int)id;
            }
            ViewData["Candidates"] = Database.Instance.GetAllOfType<Candidate>();
            return View("Index", Database.Instance.GetAllOfType<Candidate>());
        }

        public ActionResult SelectSingleCandidate(int? candidateId)
        {
            if (candidateId == null)
            {
                return View("Index", Database.Instance.GetAllOfType<Candidate>()); 
            }
            ViewData["testTemplates"] = Database.Instance.GetAllOfType<TestTemplate>();
            return View("Details", Database.Instance.GetById<Candidate>((int)candidateId));
        }

        public ActionResult InsertCandidate(Candidate instertedCandidate)
        {
            if (instertedCandidate.Name.Trim().Length == 0)
            {
                return RedirectToAction("Index");
            }
            Candidate newCandidate = Database.Instance.MakeNew<Candidate>(instertedCandidate.Name);
            Update(newCandidate, instertedCandidate, false);
            ViewData["id"] = newCandidate.Id;
            ViewData["testTemplates"] = Database.Instance.GetAllOfType<TestTemplate>();
            return View("Details", newCandidate);
        }

        public ActionResult UpdateCandidate(int id, Candidate candidate, bool? removeOldFile)
        {
            Candidate toUpdate = Database.Instance.GetById<Candidate>(id);
            if (toUpdate == null) return RedirectToAction("Index");
            Update(toUpdate, candidate, removeOldFile ?? false);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateSingleCandidate(int candidateId, Candidate candidate, bool? removeOldFile)
        {
            Candidate toUpdate = Database.Instance.GetById<Candidate>(candidateId);
            if (toUpdate == null) return RedirectToAction("Index");
            Update(toUpdate, candidate, removeOldFile ?? false);
            return RedirectToAction("Details", toUpdate);
        }

        private void Update(Candidate old, Candidate _new, bool removeOldFile)
        {
            old.Name = _new.Name;
            old.EmailAddress = _new.EmailAddress;
            old.PhoneNumber = _new.PhoneNumber;
            old.Inactive = _new.Inactive;
            if (_new.PhoneNumber.Number != null) old.PhoneNumber = _new.PhoneNumber;
            HttpPostedFileBase cvFile = Request.Files["file"];
            if (removeOldFile || cvFile != null)
            {
                DeleteCv(old);
            }
            if (cvFile != null && cvFile.ContentLength > 0)
            {
                old.CV = Database.Instance.MakeNew<File>(cvFile);
            }
            Database.Instance.Save(old);
        }

        public ActionResult AssingToTemplate(int? id, int? templatesToChoseId)
        {
            if (id == null || templatesToChoseId == null)
            {
                return RedirectToAction("Details", id);
            }
            else
            {
                if (!Database.Instance.GetById<Candidate>((int)id).Inactive)
                {
                    TestTemplate template = Database.Instance.GetById<TestTemplate>((int)templatesToChoseId);
                    Candidate candidate = Database.Instance.GetById<Candidate>((int)id);
                    Database.Instance.MakeNew<Test>(template, candidate);
                }
                return RedirectToAction("Details", new { id = (int)id });
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", Database.Instance.GetAllOfType<Candidate>());
            }
            else
            {
                //QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                //IList<QuestionGroup> allGroups = Database.Instance.GetAllOfType<QuestionGroup>();
                //ViewData["groupsToAdd"] = allGroups.Except(content.InGroups);
                ViewData["id"] = id;
                ViewData["testTemplates"] = Database.Instance.GetAllOfType<TestTemplate>();
                ViewData["tests"] = Database.Instance.GetAllOfType<Test>(t => t.Candidate.Id == (int)id);
                return View(Database.Instance.GetById<Candidate>((int)id));
            }
        }

        private void DeleteCv(Candidate candidate)
        {
            File file = candidate.CV;
            if (file != null)
            {
                candidate.CV = null;
                Database.Instance.Delete(file);
            }
        }
    }
}

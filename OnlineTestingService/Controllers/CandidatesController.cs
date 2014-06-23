using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic.Entities;
using OnlineTestingService.BusinessLogic;
using System.Reflection;
using System.Web.Security;
using OnlineTestingService.Models;

namespace OnlineTestingService.Controllers
{
    [Authorize(Roles=Models.User.CANDIDATE_MANAGER + "," + Models.User.ADMIN)]
    public class CandidatesController : BaseController
    {
        //
        // GET: /Candidates/

        public ActionResult Index()
        {
            return View(Database.GetAllOfType<Candidate>(c => c.Inactive == false));
        }

        public ActionResult SelectCandidate(int? id)
        {
            if (id != null)
            {
                ViewData["id"] = (int)id;
            }
            ViewData["Candidates"] = Database.GetAllOfType<Candidate>();
            return View("Index", Database.GetAllOfType<Candidate>());
        }

        public ActionResult SelectSingleCandidate(int? candidateId)
        {
            if (candidateId == null)
            {
                return View("Index", Database.GetAllOfType<Candidate>()); 
            }
            ViewData["testTemplates"] = Database.GetAllOfType<TestTemplate>();
            return View("Details", Database.GetById<Candidate>((int)candidateId));
        }

        public ActionResult InsertCandidate(Candidate instertedCandidate)
        {
            if (instertedCandidate.Name.Trim().Length == 0)
            {
                return RedirectToAction("Index");
            }
            Candidate newCandidate = Database.MakeNew<Candidate>(instertedCandidate.Name);
            Update(newCandidate, instertedCandidate, false);
            ViewData["id"] = newCandidate.Id;
            ViewData["testTemplates"] = Database.GetAllOfType<TestTemplate>();
            return View("Details", newCandidate);
        }

        public ActionResult UpdateCandidate(int id, Candidate candidate, bool? removeOldFile)
        {
            Candidate toUpdate = Database.GetById<Candidate>(id);
            if (toUpdate == null) return RedirectToAction("Index");
            Update(toUpdate, candidate, removeOldFile ?? false);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateSingleCandidate(int candidateId, Candidate candidate, bool? removeOldFile)
        {
            Candidate toUpdate = Database.GetById<Candidate>(candidateId);
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
                old.CV = Database.MakeNew<File>(cvFile);
            }
            Database.Save(old);
        }

        public ActionResult AssingToTemplate(int? id, int? templatesToChoseId)
        {
            if (id == null || templatesToChoseId == null)
            {
                return RedirectToAction("Details", id);
            }
            else
            {
                if (!Database.GetById<Candidate>((int)id).Inactive)
                {
                    TestTemplate template = Database.GetById<TestTemplate>((int)templatesToChoseId);
                    Candidate candidate = Database.GetById<Candidate>((int)id);
                    Database.MakeNew<Test>(template, candidate);
                }
                return RedirectToAction("Details", new { id = (int)id });
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", Database.GetAllOfType<Candidate>());
            }
            else
            {
                //QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                //IList<QuestionGroup> allGroups = Database.Instance.GetAllOfType<QuestionGroup>();
                //ViewData["groupsToAdd"] = allGroups.Except(content.InGroups);
                ViewData["id"] = id;
                ViewData["testTemplates"] = Database.GetAllOfType<TestTemplate>();
                ViewData["tests"] = Database.GetAllOfType<Test>(t => t.Candidate.Id == (int)id);
                return View(Database.GetById<Candidate>((int)id));
            }
        }

        private void DeleteCv(Candidate candidate)
        {
            File file = candidate.CV;
            if (file != null)
            {
                candidate.CV = null;
                Database.Delete(file);
            }
        }

        public ActionResult Promote(int id)
        {
            var userId = Guid.NewGuid();
            var status = new MembershipCreateStatus();
            
            var candidate = Database.GetById<Candidate>(id);
            
            var user = new Models.User();
            user.UserID = userId;
            user.UserName = candidate.EmailAddress.Address;
            user.Email = candidate.EmailAddress.Address;
            user.IsCandidate = true;

            UserManagement.GetInstance().AddUser(user, out status);
            if (status == MembershipCreateStatus.Success)
            {
                candidate.User = Database.GetByGuid<OnlineTestingService.BusinessLogic.Entities.User>(userId);
                Database.Save(candidate);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Invite")]
        public ActionResult Invite_GET(int id, Guid testId)
        {
            var candidate = Database.GetById<Candidate>(id);

            var model = new InviteCandidateViewModel
            {
                CandidateId = id,
                CandidateName = candidate.Name,
                TestId = testId,
                Date = DateTime.Now
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Invite")]
        public ActionResult Invite_POST(InviteCandidateViewModel model)
        {
            var scheduleItem = new ScheduleItem
            {
                Candidate = Database.GetById<Candidate>(model.CandidateId),
                Test = Database.GetByGuid<Test>(model.TestId),
                Host = CurrentUser,
                Date = model.Date,
                Description = model.Description
            };

            Database.MakeNew(scheduleItem);

            return RedirectToAction("Details", new { id = model.CandidateId });
        }
    }
}

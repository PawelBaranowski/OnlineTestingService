using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic.Entities;
using OnlineTestingService.BusinessLogic;

namespace OnlineTestingService.Controllers
{
    [Authorize(Roles=Models.User.TEST_DEFINER + "," + Models.User.ADMIN)]
    public class QuestionsController : Controller
    {
        //
        // GET: /Questions/

        public ActionResult Index()
        {
            //IList<QuestionContent> toShow = Database.Instance.GetAllOfType<QuestionContent>();
            ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
            return View(Database.Instance.GetAllOfType<QuestionContent>(q => q.IsDepricated == false));
        }

        public ActionResult SelectQuestion()
        {
            ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
            return View("Index", Database.Instance.GetAllOfType<QuestionContent>(q => q.IsDepricated == false));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", Database.Instance.GetAllOfType<QuestionContent>(q => q.IsDepricated == false));
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                IList<QuestionGroup> allGroups = Database.Instance.GetAllOfType<QuestionGroup>();
                ViewData["groupsToAdd"] = allGroups.Except(content.InGroups);
                ViewData["id"] = id;
                return View(Database.Instance.GetById<QuestionContent>((int)id));
            }
        }

        public ActionResult InsertQuestion(QuestionContent instertedQuestion)
        {
            QuestionContent newQuestion = Database.Instance.MakeNew<QuestionContent>(instertedQuestion.Content);
            newQuestion.Mandatory = instertedQuestion.Mandatory;
            newQuestion.Time = instertedQuestion.Time;
            Database.Instance.Save(newQuestion);
            ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
            return RedirectToAction("Details", newQuestion);
        }

        public ActionResult UpdateQuestion(int? id, QuestionContent updatedQuestion)
        {
            if (id == null || updatedQuestion == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                content.IsDepricated = true;
                QuestionContent newContent = Database.Instance.MakeNew<QuestionContent>(updatedQuestion.Content);
                newContent.Time = updatedQuestion.Time;
                newContent.Mandatory = updatedQuestion.Mandatory;
                foreach (QuestionGroup group in content.InGroups)
                {
                    group.AddQuestion(newContent);
                }
                //ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
                return RedirectToAction("Index");
            }
        }

        public ActionResult UpdateSingleQuestion(int? id, QuestionContent updatedQuestion)
        {
            if (id == null || updatedQuestion == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                content.IsDepricated = true;
                QuestionContent newContent = Database.Instance.MakeNew<QuestionContent>(updatedQuestion.Content);
                newContent.Time = updatedQuestion.Time;
                newContent.Mandatory = updatedQuestion.Mandatory;
                foreach (QuestionGroup group in content.InGroups)
                {
                    group.AddQuestion(newContent);
                }
                //ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
                return RedirectToAction("Details", new { id = newContent.Id });
            }
        }

        public ActionResult SaveContent(int? id, string questionContent, bool isMandatory, int time)
        {
            if (id == null)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                content.IsDepricated = true;
                QuestionContent newContent = Database.Instance.MakeNew<QuestionContent>(questionContent);
                newContent.Time = time;
                newContent.Mandatory = isMandatory;
                foreach (QuestionGroup group in content.InGroups)
                {
                    group.AddQuestion(newContent);
                }
                ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
                return RedirectToAction("Details", new {id = newContent.Id});
            }
        }

        public ActionResult DeleteQuestion(int? id, QuestionContent deletedQuestion)
        {
            if (id == null || deletedQuestion == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                content.IsDepricated = true;
                Database.Instance.Save(content);
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddToGroup(int? id, int? groupToAddId)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (groupToAddId == null)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)groupToAddId);
                group.AddQuestion(content);
                Database.Instance.Save(group);
                //Database.Instance.Save(content);
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult DeleteFromGroup(int? id, int? groupId)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            if (groupId == null)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                QuestionContent content = Database.Instance.GetById<QuestionContent>((int)id);
                QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)groupId);
                group.RemoveQuestion(content);
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult SelectGroup()
        {
            ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
            return View("Index", Database.Instance.GetAllOfType<QuestionContent>());
        }

        public ActionResult InsertGroup(QuestionGroup instertedGroup)
        {
            QuestionGroup newGroup = Database.Instance.MakeNew<QuestionGroup>(instertedGroup.Name);

            Database.Instance.Save(newGroup);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateGroup(int? id, QuestionGroup updatedGroup)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)id);
            group.Name = updatedGroup.Name;
            //ViewData["questionGroups"] = Database.Instance.GetAllOfType<QuestionGroup>();
            return RedirectToAction("Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic;
using OnlineTestingService.BusinessLogic.Entities;
using Telerik.Web.Mvc.UI;
using System.Web.Routing;

namespace OnlineTestingService.Controllers
{
    [Authorize(Roles=Models.User.TEST_DEFINER + "," + Models.User.ADMIN)]
    public class TestTemplatesController : Controller
    {
        //
        // GET: /TestTemplates/

        public ActionResult Index()
        {
            return View(Database.Instance.GetAllOfType<TestTemplate>(template => template.IsDeprecated == false));
        }

        public ActionResult SelectTemplate(int? id)
        {
            return View("Index", Database.Instance.GetAllOfType<TestTemplate>(template => template.IsDeprecated == false));
        }

        public ActionResult AddTemplate(TestTemplate inserted, bool? removeOldFile)
        {
            TestTemplate newTemplate = Database.Instance.MakeNew<TestTemplate>(inserted.Name);
            HttpPostedFileBase jobOfferFile = Request.Files["file"];
            if (jobOfferFile != null && jobOfferFile.ContentLength > 0)
            {
                newTemplate.JobOffer = Database.Instance.MakeNew<File>(jobOfferFile);
            }
            Database.Instance.Save(newTemplate);
            return RedirectToAction("Details", Database.Instance.GetById<TestTemplate>(newTemplate.Id));
        }

        public ActionResult UpdateJobOffer(int? id, bool? removeOldFile)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
            HttpPostedFileBase jobOfferFile = Request.Files["file"];
            removeOldFile = removeOldFile ?? false;
            if ((bool)removeOldFile || jobOfferFile != null)
            {
                File file = template.JobOffer;
                if (file != null)
                {
                    template.JobOffer = null;
                    Database.Instance.Delete(file);
                }
            }
            if (jobOfferFile != null && jobOfferFile.ContentLength > 0)
            {
                template.JobOffer = Database.Instance.MakeNew<File>(jobOfferFile);
            }
            Database.Instance.Save(template);
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult DeleteTemplate(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            TestTemplate toDelete = Database.Instance.GetById<TestTemplate>((int)id);
            if (toDelete == null)
            {
                return RedirectToAction("Index");
            }
            toDelete.IsDeprecated = true;
            Database.Instance.Save(toDelete);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                IList<QuestionGroup> allGroups = Database.Instance.GetAllOfType<QuestionGroup>();
                ViewData["groupsToAdd"] = allGroups.Except<QuestionGroup>(template.QuestionGroups.Keys);
                ViewData["id"] = id;
                return View(template);
            }
        }

        public ActionResult AddEmail(int? id, EmailAddress email)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                if (template != null)
                {
                    template.AddEmailAddress(email.Address);
                }
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult DeleteEmail(int? id, EmailAddress email)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                if (template != null)
                {
                    template.RemoveEmailAddress(email.Address);
                    Database.Instance.Save(template);
                }
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult UpdateEmail(int? id, EmailAddress email)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ValueProviderResult result = ValueProvider.GetValue("Address");
                if (result == null) throw new NullReferenceException("Can't find value for 'Address'!");
                string[] values = (string[])result.RawValue;
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                if (template != null)
                {
                    // values[1] is the OLD value
                    template.RemoveEmailAddress(values[1]);
                    template.AddEmailAddress(email.Address);
                    Database.Instance.Save(template);
                }
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult AddGroup(int? id, int? groupToAddId)
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
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)groupToAddId);
                if (template != null && group != null)
                {
                    template.AddOrUpdateQuestionGroup(group, 0);
                    Database.Instance.Save(template);
                }
                //Request["GroupsAndNumbers-mode"] = "edit";
                //Request.Params.Add("GroupsAndNumbers-mode", "edit");
                //Request.Params.Set("GroupsAndNumbers-mode", "edit");
                //Request.QueryString.Add("GroupsAndNumbers-mode", "edit");
                Request.RequestContext.RouteData.Values.Add("GroupsAndNumbers-mode", "edit");
                
                ViewData["GroupsAndNumbers-mode"] = "edit";
                RouteValueDictionary route = new RouteValueDictionary();
                route.Add("id", id);
                route.Add("GroupsAndNumbers-mode", "edit");
                route.Add("GroupId", groupToAddId);
                return RedirectToAction("Details", route);
            }
        }

        public ActionResult UpdateGroup(int? id, int? GroupId, int? number)
        {
            if (id == null || GroupId == null || number == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)GroupId);
                if (template != null && group != null)
                {
                    if ((int)number > group.QuestionContents.Count)
                    {
                        number = group.QuestionContents.Count;
                    }
                    template.AddOrUpdateQuestionGroup(group, (int)number);
                    Database.Instance.Save(template);
                }
                return RedirectToAction("Details", new { id = id });
            }
        }

        public ActionResult DeleteGroup(int? id, int? GroupId)
        {
            if (id == null || GroupId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TestTemplate template = Database.Instance.GetById<TestTemplate>((int)id);
                QuestionGroup group = Database.Instance.GetById<QuestionGroup>((int)GroupId);
                if (template != null && group != null)
                {
                    template.RemoveQuestionGroup(group);
                    Database.Instance.Save(template);
                }
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}

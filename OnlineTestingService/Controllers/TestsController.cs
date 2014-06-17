using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic.Entities;
using OnlineTestingService.BusinessLogic;

namespace OnlineTestingService.Controllers
{
    [Authorize]
    public class TestsController : Controller
    {
        //
        // GET: /Tests/

        public ActionResult Index()
        {
            return View(Database.Instance.GetAllOfType<Test>());
        }

        public ActionResult Select()
        {
            return View("Index", Database.Instance.GetAllOfType<Test>());
        }

        [Authorize(Roles=Models.User.TEST_REVIEWER)]
        public ActionResult Review(string id, int? q)
        {
            ViewData["guid"] = id;
            Test t = null;
            try
            {
                Guid g = new Guid(id);
                t = Database.Instance.GetByGuid<Test>(g);
            }
            catch (Exception e) { return HttpNotFound("Test not found"); }
            if (t == null) return HttpNotFound("Test not found");
            t.Start();
            Database.Instance.Save(t);
            ViewData["totalQuestion"] = t.Questions.Count;
            ViewData["candidateName"] = t.Candidate.Name;
            ViewData["jobOfferName"] = t.TestTemplate.Name;

            int questionNumber = q ?? 0;
            if (questionNumber < 0) questionNumber = 0;
            if (questionNumber >= t.Questions.Count) questionNumber = t.Questions.Count - 1;

            ViewData["questionNumber"] = questionNumber;
            Question question = t.Questions[questionNumber];
            return View("Review", question);
        }

        [HttpPost]
        public ActionResult SaveCommentGrade(string id, int? questionNumber, int? nextQuestion, string comment, int? grades, bool isFinish)//, int? direction)
        {
            if (questionNumber == null)
            {
                return View("Review");
            }
            ViewData["guid"] = id;
            Test t = null;
            try
            {
                Guid g = new Guid(id);
                t = Database.Instance.GetByGuid<Test>(g);
            }
            catch (Exception e) { return HttpNotFound("Test not found"); }
            if (t == null) return HttpNotFound("Test not found");

            ViewData["totalQuestion"] = t.Questions.Count;
            ViewData["questionNumber"] = questionNumber;

            Question q = t.Questions.ElementAt((int)questionNumber);
            q.Answer.Comment = comment;
            q.Answer.Grade = grades;
            
            Database.Instance.Save(q); //Save question
            if (isFinish)
            {
                t.IsReviewed = true;
                Database.Instance.Save(t);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Review", new { id = id, q = nextQuestion ?? questionNumber });
        }
    }
}

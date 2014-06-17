using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineTestingService.BusinessLogic;
using OnlineTestingService.BusinessLogic.Entities;

namespace OnlineTestingService.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class Test_Controller : Controller
    {
        private const string ACTIVE_TEST_GUID = "activeTestGuid";

        private const string ERROR_INVALID_URL = "The requested URL is invalid!\nPlease check the link in you e-mail message";
        private const string ERROR_INVALID_FORM_DATA = "Some invalid data was passed to the login form!";
        private const string ERROR_INVALID_PASSWORD = "The password is invalid! Please try again.";
        private const string ERROR_TEST_FINISHED = "This test was previously finished and cannot be accessed anymore!";

        public ActionResult Index(string guid, int? q)
        {
            string testGuid = (string)Session[ACTIVE_TEST_GUID];
            if (guid == null && testGuid == null)
            {
                return View("Error", (object)ERROR_INVALID_URL);
            }
            if (guid != null && !guid.Equals(testGuid))
            {
                return View("Login", (object)guid);
            }
            return RedirectToAction("Test", new { id = testGuid, q = q });
        }

        [HttpPost]
        public ActionResult Login(string guid, string password)
        {
            if (guid == null || password == null)
            {
                return View("Error", (object)ERROR_INVALID_FORM_DATA);
            }
            Test test = null;
            try
            {
                test = Database.Instance.GetByGuid<Test>(new Guid(guid));
            }
            catch (Exception)
            {
                test = null;
            }
            if (test == null)
            {
                return View("Error", (object)ERROR_INVALID_URL);
            }
            if (PasswordGenerator.checkPassword(password, test.PasswordHash))
            {
                Session[ACTIVE_TEST_GUID] = guid;
                return RedirectToAction("Test", new { id = guid });
            }
            else
            {
                ViewData["errorMessage"] = ERROR_INVALID_PASSWORD;
                return View("Login", (object)guid);
            }
        }

        public ActionResult Test(string id, int? q)
        {
            if (id == null || !id.Equals(Session[ACTIVE_TEST_GUID]))
            {
                return View("Login", (object)id);
            }

            Test t = null;
            try
            {
                Guid g = new Guid(id);
                t = Database.Instance.GetByGuid<Test>(g);
            }
            catch (Exception e) { return HttpNotFound("Test not found"); }
            if (t == null) return HttpNotFound("Test not found");
            if (t.HasExpired())
            {
                t.Finish();
            }
            if (t.IsFinished)
            {
                TimeSpan delay = DateTime.Now - t.EndTime;
                if (delay.TotalMinutes <= 10.0)
                {
                    return View("Finished", t);
                }
                else
                {
                    return View("Error", ERROR_TEST_FINISHED);
                }
                
            }
            t.Start();
            Database.Instance.Save(t);
            
            ViewData["guid"] = id;
            ViewData["testName"] = t.TestTemplate.Name;
            ViewData["totalQuestion"] = t.Questions.Count;
            
            int questionNumber = q ?? 0;
            if (questionNumber < 0) questionNumber = 0;
            if (questionNumber >= t.Questions.Count) questionNumber = t.Questions.Count - 1;

            ViewData["questionNumber"] = questionNumber;
            TimeSpan time = t.EndTime - DateTime.Now;            
            ViewData["time"] = time.Hours * 3600 + time.Minutes * 60 + time.Seconds;
            Question question = t.Questions[questionNumber];
            
            return View("Test", question);
        }

        [HttpPost]
        public ActionResult SaveButton(string id, int? questionNumber, int? nextQuestion, string answer, bool? finishTest)
        {            
            if (questionNumber == null)
            {
                return View("Test");
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
            q.Answer.Content = answer;
            Database.Instance.Save(q); //Save question
            finishTest = finishTest ?? false;
            if ((bool)finishTest)
            {
                t.Finish();
            }
            return RedirectToAction("Test", new { id = id, q = nextQuestion ?? questionNumber});
        }

        public ActionResult FinishTest(string id, string answer, int? questionNumber)
        {
            Test t = null;
            try
            {
                Guid g = new Guid(id);
                t = Database.Instance.GetByGuid<Test>(g);
            }
            catch (Exception e) { return HttpNotFound("Test not found"); }
            if (t == null) return HttpNotFound("Test not found");
            if (questionNumber == null)
            {
                t.Finish();
                Database.Instance.Save(t);
                return View("Finished");
            }
            Question q = t.Questions.ElementAt((int)questionNumber);
            q.Answer.Content = answer;
            Database.Instance.Save(q);
            t.Finish();
            Database.Instance.Save(t);
            return View("Finished");
        }
    }
}

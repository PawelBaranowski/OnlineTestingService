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
    [Authorize]
    public class FileController : Controller
    {
        public ActionResult Get(int id)
        {
            File file = Database.Instance.GetById<File>(id);
            FileResult fileResult = new FileContentResult(file.Content, file.ContentType);
            fileResult.FileDownloadName = file.Name;
            return fileResult;
        }
    }
}

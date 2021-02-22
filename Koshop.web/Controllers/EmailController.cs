using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koshop.web.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        [ChildActionOnly]
        public ActionResult ActiveUser()
        {
            return PartialView();
        }
    }
}
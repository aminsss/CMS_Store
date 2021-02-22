using Koshop.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Koshop.web.Controllers
{

    public class PageController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Page
        [Route("page/{id}")]
        public ActionResult Index(string id)
        {
            return View(db.Menus.Where(x => x.PageName == id).FirstOrDefault());
        }
    }
}
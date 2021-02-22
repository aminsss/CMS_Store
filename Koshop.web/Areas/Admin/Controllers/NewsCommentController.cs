using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using PagedList;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class NewsCommentController : Controller
    {
        private INewsCommentService _newsCommentService;

        public NewsCommentController(INewsCommentService newsCommentService)
        {
            _newsCommentService = newsCommentService;
        }

        // GET: Admin/NewsComment
        public ActionResult Index(bool? isActive = false, int page = 1, int _pageSize = 5)
        {
            var newsComment = _newsCommentService.GetAll(isActive);
            return View(newsComment.ToPagedList(page,_pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ChangeStatus(int commentId, bool isActive)
        {
            _newsCommentService.ChangeStatus(commentId, isActive);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            _newsCommentService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
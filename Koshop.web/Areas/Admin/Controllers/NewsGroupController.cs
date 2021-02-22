using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class NewsGroupController : Controller
    {
        private INewsGroupService _newsGroupService;

        public NewsGroupController(INewsGroupService newsGroupService)
        {
            _newsGroupService = newsGroupService;
        }
        // GET: Admin/NewsGroup
        public ActionResult Index(int page=1,int pageSize =1000,string searchString = "")
        {
            var newsGroup = _newsGroupService.GetBySearch(page, pageSize, searchString);
            return View(newsGroup.Records);
        }

        // GET: Admin/NewsGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup newsGroup = _newsGroupService.GetById(id);
            if (newsGroup == null)
            {
                return HttpNotFound();
            }
            return View(newsGroup);
        }

        // GET: Admin/NewsGroup/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/NewsGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsGroupId,GroupTitle,ParentId,IsActive,AliasName")] NewsGroup newsGroup)
        {
            if (ModelState.IsValid)
            {
                if (newsGroup.ParentId == 0)
                {
                    newsGroup.Depth = 0;
                    newsGroup.Path = "0";
                }
                else
                {
                    var newsGroupParent = _newsGroupService.GetById(newsGroup.ParentId);
                    newsGroup.Depth = newsGroupParent.Depth + 1;
                    newsGroup.Path = newsGroupParent.NewsGroupId + "/" + newsGroupParent.Path;
                }
                _newsGroupService.Add(newsGroup);
                return RedirectToAction("Index");
            }

            return View(newsGroup);
        }

        public JsonResult ErrorGroup(int? NewsGroupId, int? ParentId)
        {
            if (ParentId == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (NewsGroupId == ParentId)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var newsGroupParent = _newsGroupService.GetById(ParentId);
            foreach (var item in newsGroupParent.Path.Split('/'))
            {
                if (item == ((NewsGroupId).ToString()))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/NewsGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup newsGroup = _newsGroupService.GetById(id);
            if (newsGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(newsGroup);
        }

        // POST: Admin/NewsGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsGroupId,GroupTitle,ParentId,IsActive,AliasName")] NewsGroup newsGroup)
        {
            if (ModelState.IsValid)
            {
                if (newsGroup.NewsGroupId == newsGroup.ParentId)
                {
                    ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
                    return View(newsGroup);
                }
                if (newsGroup.ParentId == 0)
                {
                    newsGroup.Depth = 0;
                    newsGroup.Path = "0";
                }
                else
                {
                    var newGroupParent = _newsGroupService.GetById(newsGroup.ParentId);
                    foreach (var item in newGroupParent.Path.Split('/'))
                    {
                        if (item == ((newsGroup.NewsGroupId).ToString()))
                        {
                            ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                            return View(newsGroup);
                        }
                    }
                    newsGroup.Depth = newGroupParent.Depth + 1;
                    newsGroup.Path = newGroupParent.NewsGroupId + "/" + newGroupParent.Path;
                }
                _newsGroupService.Edit(newsGroup);
                return RedirectToAction("Index");
            }
            return View(newsGroup);
        }

        // GET: Admin/NewsGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsGroup newsGroup = _newsGroupService.GetById(id);
            if (newsGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(newsGroup);
        }

        // POST: Admin/NewsGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            _newsGroupService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        
        public ViewResult GrouposOfNews()
        {
            return View(_newsGroupService.NewsGroups().ToList());
        }

        public JsonResult UniqueAliasName(string AliasName, int? NewsGroupId)
        {
            if (_newsGroupService.UniqueAlias(AliasName, NewsGroupId))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

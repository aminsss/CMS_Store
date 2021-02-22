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
    public class MenuGroupsController : Controller
    {
        //private AppDbContext db = new AppDbContext();
        private IMenuGroupService _menuGroupService;

        public MenuGroupsController(IMenuGroupService menuGroupService)
        {
            _menuGroupService = menuGroupService;
        }

        // GET: Admin/MenuGroups
        public ActionResult Index(int page=1,int pageSize = 1000,string searchstring="")
        {
            return View(_menuGroupService.GetBySearch(page,pageSize, searchstring).Records);
        }

        // GET: Admin/MenuGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuGroup menuGroups = _menuGroupService.GetById(id);
            if (menuGroups == null)
            {
                return HttpNotFound();
            }
            return View(menuGroups);
        }

        // GET: Admin/MenuGroups/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Admin/MenuGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuGroup menuGroups)
        {
            if (ModelState.IsValid)
            {
                _menuGroupService.Add(menuGroups);
                return RedirectToAction("Index");
            }

            return View(menuGroups);
        }

        // GET: Admin/MenuGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuGroup menuGroup = _menuGroupService.GetById(id);
            if (menuGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(menuGroup);
        }

        // POST: Admin/MenuGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( MenuGroup menuGroup)
        {
            if (ModelState.IsValid)
            {
                _menuGroupService.Edit(menuGroup);
                return RedirectToAction("Index");
            }
            return View(menuGroup);
        }

        // GET: Admin/MenuGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuGroup menuGroups = _menuGroupService.GetById(id);
            if (menuGroups == null)
            {
                return HttpNotFound();
            }
            return PartialView(menuGroups);
        }

        // POST: Admin/MenuGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            _menuGroupService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

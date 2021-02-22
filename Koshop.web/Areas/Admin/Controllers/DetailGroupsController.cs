using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class DetailGroupsController : Controller
    {
        private IProductGroupService _productGroupService;
        private IDetailGroupService _detailGroupService; 

        public DetailGroupsController (IProductGroupService productGroupService,IDetailGroupService detailGroupService)
        {
            _detailGroupService = detailGroupService;
            _productGroupService = productGroupService;
        }


        // GET: Admin/AttributGrps
        public ActionResult Index(int page = 1, int pageSize = 100, string searchString = "")
        {
            var list = _detailGroupService.GetBySearch(page, pageSize, searchString);
            return View(list.Records);
        }


        // GET: Admin/AttributGrps/Create
        public ActionResult Create()
        {
            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle");
            return PartialView();
        }

        // POST: Admin/AttributGrps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetailGroupId,Name,ProductGroupId")] DetailGroup detailGroups)
        {
            if (ModelState.IsValid)
            {
                _detailGroupService.Add(detailGroups);
                return RedirectToAction("Index");
            }

            ViewBag.PrProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", detailGroups.ProductGroupId);
            return View(detailGroups);
        }

        // GET: Admin/DetailGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailGroup detailGroups = _detailGroupService.GetById(id);
            if (detailGroups == null)
            {
                return HttpNotFound();
            }
            ViewBag.PrProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", detailGroups.ProductGroupId);
            return PartialView(detailGroups);
        }

        // POST: Admin/DetailGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DetailGroupId,Name,ProductGroupId")] DetailGroup detailGroups)
        {
            if (ModelState.IsValid)
            {
                _detailGroupService.Edit(detailGroups);
                return RedirectToAction("Index");
            }
            ViewBag.PrProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", detailGroups.ProductGroupId);
            return View(detailGroups);
        }
       
        
        // GET: Admin/AttributGrps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailGroup detailGroups = _detailGroupService.GetById(id);
            if (detailGroups == null)
            {
                return HttpNotFound();
            }
            return PartialView(detailGroups);
        }

        // POST: Admin/AttributGrps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _detailGroupService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}

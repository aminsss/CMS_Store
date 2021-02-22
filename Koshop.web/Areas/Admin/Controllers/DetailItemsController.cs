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
    public class DetailItemsController : Controller
    {
        private IDetailItemService _detailItemService;
        private IDetailGroupService _detailGroupService;

        public DetailItemsController(IDetailGroupService detailGroupService,IDetailItemService detailItemService)
        {
            _detailItemService = detailItemService;
            _detailGroupService = detailGroupService;
        }

        // GET: Admin/DetailItems
        public ActionResult Index(int? id)
        {
            var list = _detailItemService.GetByDetGrpId(id);
            return View(list.Records);
        }



        // GET: Admin/DetailItems/Create
        public ActionResult Create(int? id)
        {
            ViewBag.DetailGroupId = new SelectList(_detailGroupService.DetailGroup(), "DetailGroupId", "Name", _detailGroupService.GetById(id));
            return PartialView();
        }

        // POST: Admin/DetailItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DetailItem DetailItems)
        {
            if (ModelState.IsValid)
            {
                _detailItemService.Add(DetailItems);
                return RedirectToAction("Index/" + Url.RequestContext.RouteData.Values["id"]);
            }

            ViewBag.DetailGroupId = new SelectList(_detailGroupService.DetailGroup(), "DetailGroupId", "Name", DetailItems.DetailGroupId);
            return View(DetailItems);
        }

        // GET: Admin/DetailItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailItem DetailItems = _detailItemService.GetById(id);
            if (DetailItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.DetailGroupId = new SelectList(_detailGroupService.DetailGroup(), "DetailGroupId", "Name", DetailItems.DetailGroupId);
            return PartialView(DetailItems);
        }

        // POST: Admin/DetailItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DetailItem DetailItems)
        {
            if (ModelState.IsValid)
            {
                _detailItemService.Edit(DetailItems);
                return RedirectToAction("Index/" + DetailItems.DetailGroupId);
            }
            ViewBag.DetailGroupId = new SelectList(_detailGroupService.DetailGroup(), "DetailGroupId", "Name", DetailItems.DetailGroupId);
            return View(DetailItems);
        }

        // GET: Admin/DetailItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetailItem DetailItems = _detailItemService.GetById(id);
            if (DetailItems == null)
            {
                return HttpNotFound();
            }
            return View(DetailItems);
        }

        // POST: Admin/DetailItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _detailItemService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

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
using Koshop.ViewModels;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class AttributItemsController : Controller
    {
        private IAttributeGrpService _attributeGrpService;
        private IAttributeItemService _attributeItemService;

        public AttributItemsController(IAttributeGrpService attributeGrpService,IAttributeItemService attributeItemService)
        {
            _attributeGrpService = attributeGrpService;
            _attributeItemService = attributeItemService;
        }

        // GET: Admin/AttributItems
        public ActionResult Index(int? id)
        {
            var list = _attributeItemService.GetByAttrGrpId(id).Records;
            return View(list);
        }


        // GET: Admin/AttributItems/Create
        public ActionResult Create(int? id)
        {
            ViewBag.AttributGrpId = new SelectList(_attributeGrpService.GetAllAttributeGrp(), "AttributGrpId", "Name",_attributeGrpService.GetById(id));
            return PartialView();
        }

        // POST: Admin/AttributItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AttributItem attributItem)
        {
            if (ModelState.IsValid)
            {
                _attributeItemService.Add(attributItem);
                return RedirectToAction("Index/"+Url.RequestContext.RouteData.Values["id"]);
            }

            ViewBag.AttributGrpId = new SelectList(_attributeGrpService.GetAllAttributeGrp(), "AttributGrpId", "Name", attributItem.AttributGrpId);
            return View(attributItem);
        }

        // GET: Admin/AttributItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributItem attributItem = _attributeItemService.GetById(id);
            if (attributItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.AttributGrpId = new SelectList(_attributeGrpService.GetAllAttributeGrp(), "AttributGrpId", "Name", _attributeItemService.GetByAttrGrpId(id));
            return PartialView(attributItem);
        }

        // POST: Admin/AttributItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AttributItem attributItem)
        {
            if (ModelState.IsValid)
            {
                _attributeItemService.Edit(attributItem);
                return RedirectToAction("Index/" + attributItem.AttributGrpId);
            }
            ViewBag.AttributGrpId = new SelectList(_attributeGrpService.GetAllAttributeGrp(), "AttributGrpId", "Name", attributItem.AttributGrpId);
            return View(attributItem);
        }

        // GET: Admin/AttributItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributItem attributItem = _attributeItemService.GetById(id);
            if (attributItem == null)
            {
                return HttpNotFound();
            }
            return PartialView(attributItem);
        }

        // POST: Admin/AttributItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttributItem attributItem = _attributeItemService.GetById(id);
            _attributeItemService.Delete(attributItem);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}

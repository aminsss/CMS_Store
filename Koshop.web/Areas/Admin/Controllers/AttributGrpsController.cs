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
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;
namespace Koshop.web.Areas.Admin.Controllers
{
    public class AttributGrpsController : Controller
    {
        private IAttributeGrpService _attributeGrpService;
        private IProductGroupService _productGroupService;

        public AttributGrpsController(IAttributeGrpService attributeGrpService,IProductGroupService productGroupService)
        {
            _attributeGrpService = attributeGrpService;
            _productGroupService = productGroupService;
        }
        // GET: Admin/AttributGrps
        public ActionResult Index(int page =1 , int pageSize = 100 ,string searchString = "")
        {
            var attributGrp = _attributeGrpService.GetBySearch(page, pageSize, searchString);
            return View(attributGrp.Records);
        }

        [HttpGet]
        public ActionResult GetAttrGrps(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = _attributeGrpService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               //Profile = obj.Profile,
                               //moblie = obj.moblie,
                               //UserId = obj.UserId,
                               //RoleName = obj.Role.RoleName,
                               //Name = obj.Name,
                               //ISActive = obj.ISActive,
                               //Email = obj.Email,
                               //AddedDate = obj.AddedDate,
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/AttributGrps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributGrp attributGrp = _attributeGrpService.GetById(id);
            if (attributGrp == null)
            {
                return HttpNotFound();
            }
            return View(attributGrp);
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
        public ActionResult Create([Bind(Include = "AttributGrpId,Name,ProductGroupId,Attr_type")] AttributGrp attributGrp)
        {
            if (ModelState.IsValid)
            {
                _attributeGrpService.Add(attributGrp);
                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributGrp attributGrp = _attributeGrpService.GetById(id);
            if (attributGrp == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", attributGrp.ProductGroupId);
            return PartialView(attributGrp);
        }

        // POST: Admin/AttributGrps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AttributGrpId,Name,ProductGroupId,Attr_type")] AttributGrp attributGrp)
        {
            if (ModelState.IsValid)
            {
                _attributeGrpService.Edit(attributGrp);
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", attributGrp.ProductGroupId);
            return View(attributGrp);
        }

        // GET: Admin/AttributGrps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttributGrp attributGrp = _attributeGrpService.GetById(id);
            if (attributGrp == null)
            {
                return HttpNotFound();
            }
            return PartialView(attributGrp);
        }

        // POST: Admin/AttributGrps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _attributeGrpService.Delete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GroupsOfProduct()
        {
            return PartialView(_productGroupService.ProductGroups());
        }

    }
}

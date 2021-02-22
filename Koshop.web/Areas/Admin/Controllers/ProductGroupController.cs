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
    public class ProductGroupController : Controller
    {
        private IProductGroupService _productGroupService;

        public ProductGroupController(IProductGroupService productGroupService)
        {
            _productGroupService = productGroupService;
        }

        // GET: Admin/ProductGroup
        public ActionResult Index(int page=1,int pageSize=1000,string searchString="")
        {
            return View(_productGroupService.GetBySearch(page,pageSize,searchString).Records);
        }

        // GET: Admin/ProductGroup/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        // GET: Admin/ProductGroup/Create
        public ActionResult Create()
        {
            return  PartialView();
        }

        // POST: Admin/ProductGroup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductGroupId,GroupTitle,ParentId,IsActive,AliasName,type")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {

                if (productGroup.ParentId == 0)
                {
                    productGroup.Depth = 0;
                    productGroup.Path = "0";
                }
                else
                {
                    var product_Group = _productGroupService.GetById(productGroup.ParentId);
                    productGroup.Depth = product_Group.Depth + 1;
                    productGroup.Path = product_Group.ProductGroupId + "/" + product_Group.Path;
                }
                _productGroupService.Add(productGroup);
                return RedirectToAction("Index");
            }

            return View(productGroup);
        }

        // GET: Admin/ProductGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(productGroup);
        }

        // POST: Admin/ProductGroup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductGroupId,GroupTitle,ParentId,IsActive,AliasName,type")] ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                if (productGroup.ProductGroupId == productGroup.ParentId)
                {
                    ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
                    return View(productGroup);
                }
                if (productGroup.ParentId == 0)
                {
                    productGroup.Depth = 0;
                    productGroup.Path = "0";
                }
                else
                {
                    var NewParent_Group = _productGroupService.GetById(productGroup.ParentId);
                    foreach (var item in NewParent_Group.Path.Split('/'))
                    {
                        if (item == ((productGroup.ProductGroupId).ToString()))
                        {
                            ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                            return View(productGroup);
                        }
                    }
                    productGroup.Depth = NewParent_Group.Depth + 1;
                    productGroup.Path = NewParent_Group.ProductGroupId + "/" + NewParent_Group.Path;
                }
                _productGroupService.Edit(productGroup);

                
                return RedirectToAction("Index");
                
            }
            return View(productGroup);
        }

        public JsonResult ErrorGroup(int? ProductGroupId, int? ParentId)
        {
            if (ParentId == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            if (ProductGroupId == ParentId)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var product_Group = _productGroupService.GetById(ParentId);
            foreach (var item in product_Group.Path.Split('/'))
            {
                if (item == ((ProductGroupId).ToString()))
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/ProductGroup/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = _productGroupService.GetById(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return PartialView(productGroup);
        }

        // POST: Admin/ProductGroup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            ProductGroup productGroup = _productGroupService.GetById(id);
            _productGroupService.Delete(productGroup);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GroupsOfProduct()
        {
            return PartialView(_productGroupService.ProductGroups());
        }

        public ActionResult GroupsOfModel()
        {
            return PartialView(_productGroupService.GetByType("car"));
        }

        public JsonResult UniqueAlias(string AliasName, int? ProductGroupId)
        {
            if (_productGroupService.UniqueAlias(AliasName,ProductGroupId))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

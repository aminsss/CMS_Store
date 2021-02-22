using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class MenusController : Controller
    {
        private IMenuService _menuService;
        private IMenuGroupService _menuGroupService;
        private IProductGroupService _productGroupService;
        private INewsGroupService _newsGroupService;

        public MenusController(IMenuGroupService menuGroupService,IMenuService menuService,IProductGroupService productGroupService,INewsGroupService newsGroupService)
        {
            _menuGroupService = menuGroupService;
            _menuService = menuService;
            _productGroupService = productGroupService;
            _newsGroupService = newsGroupService;
        }
        // GET: Admin/Menus
        public ActionResult Index(int? id)
        {
            return View(_menuService.GetByMenuGroup(id).Records);
        }


        // GET: Admin/Menus/Create
        public ActionResult Create(int? id)
        {
            ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu menu)
        {
            //[Bind(Include = "MenuID,MenuTitle,Depth,Path,IsActive,DisplayOrder,ParentId,Description,PageContetnt,GroupID")] 
            if (ModelState.IsValid)
            {
                _menuService.Add(menu);
                return RedirectToAction("Index/"+menu.MenuGroupId);
            }

            ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile", menu.MenuGroupId);
            return View(menu);
        }

        // GET: Admin/Menus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = _menuService.GetById(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile",menu.MenuGroupId);
            return View(menu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Menu menu, int? pastDisOrder,int? pastParentId,int? pastGroupId)
        {
            ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile", menu.MenuGroupId);
            if (ModelState.IsValid)
            {
                if (menu.MenuId == menu.ParentId)
                {
                    ModelState.AddModelError("ParentId", "نمی توانید گروه فعلی را برای گروه والد انتخاب کنید");
                    return View(menu);
                }
                if (menu.ParentId == 0)
                {
                    menu.Depth = 0;
                    menu.Path = "0";
                }
                else
                {
                    var newParent_Menu = _menuService.GetById(menu.ParentId);
                    foreach (var item in newParent_Menu.Path.Split('/'))
                    {
                        if (item == ((menu.MenuId).ToString()))
                        {
                            ModelState.AddModelError("ParentId", "نمی توانید از زیر گروه های این گروه انتخاب کنید");
                            return View(menu);
                        }
                    }
                    menu.Depth = newParent_Menu.Depth + 1;
                    menu.Path = newParent_Menu.MenuId + "/" + newParent_Menu.Path;
                }
                _menuService.Edit(menu, pastDisOrder, pastParentId, pastGroupId);
                return RedirectToAction("Index/" + menu.MenuGroupId);
            }
            return View(menu);
        }

        // GET: Admin/Menus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = _menuService.GetById(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return PartialView(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Menu menu = _menuService.GetById(id);
            _menuService.Delete(menu);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OrderOfMenus(int? id)
        {
            return View(_menuService.GetByMenuGroup(id).Records);
        }

        public JsonResult UniquePageName(string PageName, int? MenuId)
        {
            if (_menuService.UniquePageName(PageName,MenuId))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GrouposOfNews()
        {
            return PartialView(_newsGroupService.NewsGroups().ToList());
        }

        public ActionResult GroupsOfProduct()
        {
            return PartialView(_productGroupService.ProductGroups().ToList());
        }

        public ActionResult DisplayOrder(int? id)
        {
            return PartialView(_menuService.GetByParentId(id));
        }
    }
}

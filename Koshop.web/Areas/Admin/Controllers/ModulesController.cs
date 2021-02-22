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
    public class ModulesController : Controller
    {
        private IModuleService _moduleService;
        private IComponentService _componentService;
        private IMenuGroupService _menuGroupService;

        public ModulesController(IModuleService moduleService,IComponentService componentService,IMenuGroupService menuGroupService)
        {
            _moduleService = moduleService;
            _componentService = componentService;
            _menuGroupService = menuGroupService;
        }

        // GET: Admin/Modules
        public ActionResult Index(string searchString = "")
        {
            var modules = _moduleService.GetBySearch(searchString);
            return View(modules.Records);
        }

        public ActionResult componentList()
        {
            return PartialView(_componentService.GetAll());
        }

       
        // GET: Admin/Modules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = _moduleService.GetById(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return PartialView(module);
        }

        // POST: Admin/Modules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            _moduleService.Delete(id);
            return Json(true,JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayOrder(int? id)
        {
            return PartialView(_moduleService.GetByPositionId(id));
        }

        public ActionResult ModulePageShow(int? id)
        {
            ViewBag.moduleId = id;
            return PartialView(_menuGroupService.MenuGroup());
        }

    }
}

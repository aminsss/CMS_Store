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
    public class MenuModuleController : Controller
    {

        private IModuleService _moduleService;
        private IMenuModuleService _menuModuleService;
        private IMenuService _menuService;
        private IMenuGroupService _menuGroupService;
        private IModulePageService _modulePageService;

        public MenuModuleController(IModuleService moduleService, IMenuModuleService menuModuleService
            , IMenuService menuService,IMenuGroupService menuGroupService,IModulePageService modulePageService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _menuModuleService = menuModuleService;
            _menuGroupService = menuGroupService;
            _modulePageService = modulePageService;
        }

        public ActionResult Index()
        {
           return RedirectToAction("Index", new { Controller = "Modules" });
        }

        // GET: Admin/MenuModule/Create
        public ActionResult Create()
        {
            ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
            ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
            return View();
        }

        // POST: Admin/MenuModule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MenuModulViewModel menuModulViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = menuModulViewModel.ModuleTitle,
                    PositionId = (int)menuModulViewModel.PositionId,
                    IsActive = menuModulViewModel.IsActive,
                    Accisibility = menuModulViewModel.Accisibility,
                    ComponentId = 1,
                };


                //Method for selecting menus for modules
                foreach (var item in _menuService.menus())
                {
                    if (Request.Form["Page[" + item.MenuId.ToString() + "]"] != null)
                    {
                        ModulePage modulePage = new ModulePage()
                        {
                            MenuId = item.MenuId,
                        };
                        module.ModulePage.Add(modulePage);
                    }
                }

                //for menuModule inserting
                module.MenuModule = new MenuModule()
                {
                    MenuGroupId = menuModulViewModel.MenuGroupId,
                };

                //Add the Module
                _moduleService.Add(module);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطایی وجود دارد");
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
                return View();
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/MenuModule/Edit/5
        public ActionResult Edit(int? id)
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
            else
            {
                MenuModulViewModel menuModulViewModel = new MenuModulViewModel()
                {
                    ModuleId = module.ModuleId,
                    ModuleTitle = module.ModuleTitle,
                    IsActive = module.IsActive,
                    PositionId = module.PositionId,
                    MenuGroupId = module.MenuModule.MenuGroupId,
                    DisplayOrder = module.DisplayOrder
                };
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");

                return View(menuModulViewModel);
            }
        }

        // POST: Admin/MenuModule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MenuModulViewModel menuModulViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                Module module = _moduleService.GetById(menuModulViewModel.ModuleId);
                if (module != null)
                {
                    module.ModuleTitle = menuModulViewModel.ModuleTitle;
                    module.PositionId = (int)menuModulViewModel.PositionId;
                    module.IsActive = menuModulViewModel.IsActive;
                    module.Accisibility = menuModulViewModel.Accisibility;
                    module.DisplayOrder = menuModulViewModel.DisplayOrder;


                    //Method for selecting menus for modules
                    List<ModulePage> modulePageAddList = new List<ModulePage>();
                    List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                    foreach (var item in _menuService.menus())
                    {
                        if (Request.Form["Page[" + item.MenuId.ToString() + "]"] != null && !(_modulePageService.ExistModulePage(menuModulViewModel.ModuleId, item.MenuId)))
                        {
                            ModulePage modulePage = new ModulePage()
                            {
                                MenuId = item.MenuId,
                                ModuleId = menuModulViewModel.ModuleId,
                            };
                            modulePageAddList.Add(modulePage);
                        }
                        else if (Request.Form["Page[" + item.MenuId.ToString() + "]"] == null && _modulePageService.ExistModulePage(menuModulViewModel.ModuleId,item.MenuId))
                        {
                            ModulePage PageRemove = _modulePageService.GetByMenuModule(menuModulViewModel.ModuleId, item.MenuId);
                            modulePageRemoveList.Add(PageRemove);
                        }
                    }
                    _modulePageService.Add(modulePageAddList);
                    _modulePageService.Delete(modulePageRemoveList);

                    //editing MenuModule GroupId if it's changed
                    var EditMenuModule = _menuModuleService.GetByModuleId(menuModulViewModel.ModuleId);
                    if (EditMenuModule.MenuGroupId != menuModulViewModel.MenuGroupId)
                    {
                        EditMenuModule.MenuGroupId = menuModulViewModel.MenuGroupId;
                        _menuModuleService.Edit(EditMenuModule);
                    }

                    //Editing the Module
                    _moduleService.Edit(module, pastPosition, pastDisOrder);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطایی وجود دارد");
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                ViewBag.MenuGroupId = new SelectList(_menuGroupService.MenuGroup(), "MenuGroupId", "MenuTitile");
                return View();
            }
            return RedirectToAction("Index");
        }

    }
}

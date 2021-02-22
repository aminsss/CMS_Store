using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using System.Net;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class HtmlModuleController : Controller
    {
        // GET: Admin/HtmlModule


        private IModuleService _moduleService;
        private IMenuService _menuService;
        private IModulePageService _modulePageService;
        private IHtmlModuleService _htmlModuleService;

        public HtmlModuleController(IModuleService moduleService, IHtmlModuleService htmlModuleService
            , IMenuService menuService, IModulePageService modulePageService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _htmlModuleService = htmlModuleService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", new { Controller = "Modules" });
        }

        // GET: Admin/MenuModule/Create
        public ActionResult Create(int? id)
        {
            ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
            ViewBag.componentid = id;
            return View();
        }

        // POST: Admin/MenuModule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HtmlModulViewModel htmlModulViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = htmlModulViewModel.ModuleTitle,
                    PositionId = (int)htmlModulViewModel.PositionId,
                    IsActive = htmlModulViewModel.IsActive,
                    Accisibility = htmlModulViewModel.Accisibility,
                    ComponentId = Convert.ToInt32(Request.Form["componentid"])
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

                //for HtmlModule inserting
                module.HtmlModule = new HtmlModule()
                {
                    HtmlText = htmlModulViewModel.HtmlText,
                };

                //Add the Module
                _moduleService.Add(module);

            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطایی وجود دارد");
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
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
                HtmlModulViewModel htmlModulViewModel = new HtmlModulViewModel()
                {
                    ModuleId = module.ModuleId,
                    ModuleTitle = module.ModuleTitle,
                    IsActive = module.IsActive,
                    PositionId = module.PositionId,
                    DisplayOrder = module.DisplayOrder,
                    HtmlText = module.HtmlModule.HtmlText,
                };
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                return View(htmlModulViewModel);
            }
        }

        // POST: Admin/MenuModule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HtmlModulViewModel htmlModulViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                Module module = _moduleService.GetById(htmlModulViewModel.ModuleId);
                if (module != null)
                {
                    module.ModuleTitle = htmlModulViewModel.ModuleTitle;
                    module.PositionId = (int)htmlModulViewModel.PositionId;
                    module.IsActive = htmlModulViewModel.IsActive;
                    module.Accisibility = htmlModulViewModel.Accisibility;
                    module.DisplayOrder = htmlModulViewModel.DisplayOrder;

                    //Method for selecting menus for modules
                    List<ModulePage> modulePageAddList = new List<ModulePage>();
                    List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                    foreach (var item in _menuService.menus())
                    {
                        if (Request.Form["Page[" + item.MenuId.ToString() + "]"] != null && !(_modulePageService.ExistModulePage(htmlModulViewModel.ModuleId, item.MenuId)))
                        {
                            ModulePage modulePage = new ModulePage()
                            {
                                MenuId = item.MenuId,
                                ModuleId = htmlModulViewModel.ModuleId,
                            };
                            modulePageAddList.Add(modulePage);
                        }
                        else if (Request.Form["Page[" + item.MenuId.ToString() + "]"] == null && _modulePageService.ExistModulePage(htmlModulViewModel.ModuleId, item.MenuId))
                        {
                            ModulePage PageRemove = _modulePageService.GetByMenuModule(htmlModulViewModel.ModuleId, item.MenuId);
                            modulePageRemoveList.Add(PageRemove);
                        }
                    }
                    _modulePageService.Add(modulePageAddList);
                    _modulePageService.Delete(modulePageRemoveList);

                    //editing HtmlModule
                    HtmlModule htmlModule  = _htmlModuleService.GetByModuleId(htmlModulViewModel.ModuleId);
                    if (htmlModule != null)
                    {
                        htmlModule.HtmlText = htmlModulViewModel.HtmlText;
                        _htmlModuleService.Edit(htmlModule);
                    }

                    //Editing the Module
                    _moduleService.Edit(module, pastPosition, pastDisOrder);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "خطایی وجود دارد");
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                return View();
            }
            return RedirectToAction("Index");
        }
        


    }
}
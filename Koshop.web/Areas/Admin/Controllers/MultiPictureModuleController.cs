using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using System.Net;
using System.IO;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class MultiPictureModuleController : Controller
    {
        // GET: Admin/HtmlModule


        private IModuleService _moduleService;
        private IMenuService _menuService;
        private IModulePageService _modulePageService;
        private IMultiPictureModuleService _multiPictureModuleService;

        public MultiPictureModuleController(IModuleService moduleService, IMultiPictureModuleService multiPictureModuleService
            , IMenuService menuService, IModulePageService modulePageService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _multiPictureModuleService = multiPictureModuleService;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string imagename = "no-photo.jpg";

                if (file != null)
                {
                    //--------------------Creating names and saving to Main sarver----------------------------------
                    imagename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("/Content/Modules/Images/") + imagename);

                    //---------------------resize Images -----------------------------------------------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                    img.Resize(Server.MapPath("/Content/Modules/Images/") + imagename, Server.MapPath("/Content/Modules/Images/thumbnail/") + imagename);
                }

                //saving new store and the images
                return Json(new { status = "Done", src = "/Content/Modules/Images/thumbnail/" + imagename, ImageName = imagename });
            }
            return Json(new { status = "Error" });
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

        // POST: Admin/MultiPictureModule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MultiPictureModuleViewModel multiPictureModuleViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = multiPictureModuleViewModel.ModuleTitle,
                    PositionId = (int)multiPictureModuleViewModel.PositionId,
                    IsActive = multiPictureModuleViewModel.IsActive,
                    Accisibility = multiPictureModuleViewModel.Accisibility,
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

                //for MultiPictureModule inserting
                module.MultiPictureModule = new MultiPictureModule()
                {
                    Cover = multiPictureModuleViewModel.Cover,
                    ModuleId = multiPictureModuleViewModel.ModuleId,
                    Description = multiPictureModuleViewModel.Description,
                    Title = multiPictureModuleViewModel.Title,
                    TitleBold = multiPictureModuleViewModel.TitleBold,
                    Link = multiPictureModuleViewModel.Link,
                    LinkMore = multiPictureModuleViewModel.LinkMore,
                    Image = multiPictureModuleViewModel.ModuleImage,
                    
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
                MultiPictureModuleViewModel htmlModulViewModel = new MultiPictureModuleViewModel()
                {
                    ModuleId = module.ModuleId,
                    ModuleTitle = module.ModuleTitle,
                    IsActive = module.IsActive,
                    PositionId = module.PositionId,
                    DisplayOrder = module.DisplayOrder,
                    Title = module.MultiPictureModule.Title,
                    TitleBold = module.MultiPictureModule.TitleBold,
                    Link = module.MultiPictureModule.Link,
                    LinkMore = module.MultiPictureModule.LinkMore,
                    Cover = module.MultiPictureModule.Cover,
                    Description = module.MultiPictureModule.Description,
                    ModuleImage = module.MultiPictureModule.Image,
                };
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                return View(htmlModulViewModel);
            }
        }

        // POST: Admin/MenuModule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MultiPictureModuleViewModel multiPictureModuleViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                Module module = _moduleService.GetById(multiPictureModuleViewModel.ModuleId);
                if (module != null)
                {
                    module.ModuleTitle = multiPictureModuleViewModel.ModuleTitle;
                    module.PositionId = (int)multiPictureModuleViewModel.PositionId;
                    module.IsActive = multiPictureModuleViewModel.IsActive;
                    module.Accisibility = multiPictureModuleViewModel.Accisibility;
                    module.DisplayOrder = multiPictureModuleViewModel.DisplayOrder;

                    //Method for selecting menus for modules
                    List<ModulePage> modulePageAddList = new List<ModulePage>();
                    List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                    foreach (var item in _menuService.menus())
                    {
                        if (Request.Form["Page[" + item.MenuId.ToString() + "]"] != null && !(_modulePageService.ExistModulePage(multiPictureModuleViewModel.ModuleId, item.MenuId)))
                        {
                            ModulePage modulePage = new ModulePage()
                            {
                                MenuId = item.MenuId,
                                ModuleId = multiPictureModuleViewModel.ModuleId,
                            };
                            modulePageAddList.Add(modulePage);
                        }
                        else if (Request.Form["Page[" + item.MenuId.ToString() + "]"] == null && _modulePageService.ExistModulePage(multiPictureModuleViewModel.ModuleId, item.MenuId))
                        {
                            ModulePage PageRemove = _modulePageService.GetByMenuModule(multiPictureModuleViewModel.ModuleId, item.MenuId);
                            modulePageRemoveList.Add(PageRemove);
                        }
                    }
                    _modulePageService.Add(modulePageAddList);
                    _modulePageService.Delete(modulePageRemoveList);

                    if (multiPictureModuleViewModel.ModuleImage != module.MultiPictureModule.Image && module.MultiPictureModule.Image != "no-photo.jpg")
                    {
                        if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/" + module.MultiPictureModule.Image)))
                            System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/" + module.MultiPictureModule.Image));
                        if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/thumbnail/" + module.MultiPictureModule.Image)))
                            System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/thumbnail/" + module.MultiPictureModule.Image));
                    }

                    //editing HtmlModule
                    MultiPictureModule multiPictureModule  = _multiPictureModuleService.GetByModuleId(multiPictureModuleViewModel.ModuleId);
                    if (multiPictureModule != null)
                    {
                        multiPictureModule.Title = multiPictureModuleViewModel.Title;
                        multiPictureModule.TitleBold = multiPictureModuleViewModel.TitleBold;
                        multiPictureModule.Description = multiPictureModuleViewModel.Description;
                        multiPictureModule.Cover = multiPictureModuleViewModel.Cover;
                        multiPictureModule.Link = multiPictureModuleViewModel.Link;
                        multiPictureModule.LinkMore = multiPictureModuleViewModel.LinkMore;
                        multiPictureModule.Image = multiPictureModuleViewModel.ModuleImage;
                        _multiPictureModuleService.Edit(multiPictureModule);
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

        public ActionResult ItemsList(int? id)
        {
            IList<MultiPictureItems> multiPictures =  _multiPictureModuleService.GetMultiPictureItems((int)id);
            return PartialView(multiPictures);
        }

        [HttpGet]
        public ActionResult CreateItems(int moduleId)
        {
            ViewBag.moduleId = moduleId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateItems(MultiPictureItems multiPictureItems)
        {
            _multiPictureModuleService.CreateItems(multiPictureItems);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditItems(int multiPictureItemsId)
        {
            var multiPictureItem = _multiPictureModuleService.GetItemsById(multiPictureItemsId);
            return PartialView(multiPictureItem);
        }

        [HttpPost]
        public JsonResult EditItems(MultiPictureItems multiPictureItems)
        {
            _multiPictureModuleService.EditItems(multiPictureItems);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UploadImage(int multiPictureItemsId)
        {
            var multiPictureItem = _multiPictureModuleService.GetItemsById(multiPictureItemsId);
            return PartialView(multiPictureItem);
        }

        [HttpPost]
        public JsonResult UploadImage(int id, string imageName)
        {
            var multiPictureItem = _multiPictureModuleService.GetItemsById(id);
            if (multiPictureItem.Image != imageName && multiPictureItem.Image != "no-photo.jpg")
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/" + imageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/" + imageName));
                if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/thumbnail/" + imageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/thumbnail/" + imageName));
            }
            multiPictureItem.Image = imageName;
            _multiPictureModuleService.EditItems(multiPictureItem);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteItems(int id)
        {
            MultiPictureItems multiPictureItem = _multiPictureModuleService.GetItemsById(id);
            _multiPictureModuleService.DeleteItems(multiPictureItem);
            if (multiPictureItem.Image != "no-photo.jpg")
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/" + multiPictureItem.Image)))
                    System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/" + multiPictureItem.Image));
                if (System.IO.File.Exists(Server.MapPath("/Content/Modules/Images/thumbnail/" + multiPictureItem.Image)))
                    System.IO.File.Delete(Server.MapPath("/Content/Modules/Images/thumbnail/" + multiPictureItem.Image));
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
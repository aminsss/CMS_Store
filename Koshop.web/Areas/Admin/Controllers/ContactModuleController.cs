using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;
using Koshop.DomainClasses;
using System.Net;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class ContactModuleController : Controller
    {

        private IModuleService _moduleService;
        private IMenuService _menuService;
        private IModulePageService _modulePageService;
        private IContactModuleService _contactModuleService;
        private IContactPersonService _contactPersonService;
        private IUserService _userService;

        public ContactModuleController(IModuleService moduleService, IContactModuleService contactModuleService
                , IMenuService menuService, IModulePageService modulePageService,IContactPersonService contactPersonService,IUserService userService)
        {
            _moduleService = moduleService;
            _menuService = menuService;
            _modulePageService = modulePageService;
            _contactModuleService = contactModuleService;
            _userService = userService;
            _contactPersonService = contactPersonService;
        }

        // GET: Admin/ContactModule
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
        public ActionResult Create(ContactModuleViewModel contactModuleViewModel)
        {
            if (ModelState.IsValid)
            {
                Module module = new Module()
                {
                    ModuleTitle = contactModuleViewModel.ModuleTitle,
                    PositionId = (int)contactModuleViewModel.PositionId,
                    IsActive = contactModuleViewModel.IsActive,
                    Accisibility = contactModuleViewModel.Accisibility,
                    ComponentId = 4
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

                //for ContactModule inserting
                module.ContactModule = new ContactModule()
                {
                    Email = contactModuleViewModel.Email,
                    PostCode = contactModuleViewModel.PostCode,
                    PhoneNum = contactModuleViewModel.PhoneNum,
                    MobileNum = contactModuleViewModel.MobileNum,
                    Description = contactModuleViewModel.Description,
                    Address = contactModuleViewModel.Address,
                };

                //for Users included contactModule inserting
                List<ContactPerson> contactPeople = new List<ContactPerson>();
                foreach (var item in _userService.GetAllAdmin())
                {
                    if (Request.Form["User[" + item.UserId.ToString() + "]"] != null)
                    {
                        ContactPerson contactPerson = new ContactPerson()
                        {
                            UserId = item.UserId,
                        };
                        module.ContactModule.ContactPerson.Add(contactPerson);
                    }
                }


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
                ContactModuleViewModel contactModuleViewModel = new ContactModuleViewModel()
                {
                    ModuleId = module.ModuleId,
                    ModuleTitle = module.ModuleTitle,
                    IsActive = module.IsActive,
                    PositionId = module.PositionId,
                    DisplayOrder = module.DisplayOrder,
                    Address = module.ContactModule.Address,
                    Email = module.ContactModule.Email,
                    MobileNum = module.ContactModule.MobileNum,
                    PhoneNum = module.ContactModule.PhoneNum,
                    PostCode = module.ContactModule.PostCode,
                    Description = module.ContactModule.Description
                };
                ViewBag.PositionId = new SelectList(_moduleService.Positions(), "PositionId", "PositionTitle");
                return View(contactModuleViewModel);
            }
        }

        // POST: Admin/MenuModule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactModuleViewModel contactModuleViewModel, int pastDisOrder, int pastPosition)
        {
            if (ModelState.IsValid)
            {
                Module module = _moduleService.GetById(contactModuleViewModel.ModuleId);
                if (module != null)
                {
                    module.ModuleTitle = contactModuleViewModel.ModuleTitle;
                    module.PositionId = (int)contactModuleViewModel.PositionId;
                    module.IsActive = contactModuleViewModel.IsActive;
                    module.Accisibility = contactModuleViewModel.Accisibility;
                    module.DisplayOrder = contactModuleViewModel.DisplayOrder;

                    //Method for selecting menus for modules
                    List<ModulePage> modulePageAddList = new List<ModulePage>();
                    List<ModulePage> modulePageRemoveList = new List<ModulePage>();
                    foreach (var item in _menuService.menus())
                    {
                        if (Request.Form["Page[" + item.MenuId.ToString() + "]"] != null && !(_modulePageService.ExistModulePage(contactModuleViewModel.ModuleId, item.MenuId)))
                        {
                            ModulePage modulePage = new ModulePage()
                            {
                                MenuId = item.MenuId,
                                ModuleId = contactModuleViewModel.ModuleId,
                            };
                            modulePageAddList.Add(modulePage);
                        }
                        else if (Request.Form["Page[" + item.MenuId.ToString() + "]"] == null && _modulePageService.ExistModulePage(contactModuleViewModel.ModuleId, item.MenuId))
                        {
                            ModulePage PageRemove = _modulePageService.GetByMenuModule(contactModuleViewModel.ModuleId, item.MenuId);
                            modulePageRemoveList.Add(PageRemove);
                        }
                    }
                    _modulePageService.Add(modulePageAddList);
                    _modulePageService.Delete(modulePageRemoveList);

                    //editing HtmlModule
                    ContactModule contactModule = _contactModuleService.GetByModuleId(contactModuleViewModel.ModuleId);
                    if (contactModule != null)
                    {
                        contactModule.Email = contactModuleViewModel.Email;
                        contactModule.PostCode = contactModuleViewModel.PostCode;
                        contactModule.PhoneNum = contactModuleViewModel.PhoneNum;
                        contactModule.MobileNum = contactModuleViewModel.MobileNum;
                        contactModule.Description = contactModuleViewModel.Description;
                        contactModule.Address = contactModuleViewModel.Address;
                       _contactModuleService.Edit(contactModule);
                    }

                    //for Users Editing contactModule updating
                    List<ContactPerson> contactPeopleAddList = new List<ContactPerson>();
                    List<ContactPerson> contactPeopleRemoveList = new List<ContactPerson>();
                    foreach (var item in _userService.GetAllAdmin())
                    {
                        if (Request.Form["User[" + item.UserId.ToString() + "]"] != null && !(_contactPersonService.ExistContactPerson(contactModuleViewModel.ModuleId, item.UserId)))
                        {
                            ContactPerson contactPerson = new ContactPerson()
                            {
                                UserId = item.UserId,
                                ContactModuleId = contactModuleViewModel.ModuleId,
                            };
                            contactPeopleAddList.Add(contactPerson);
                        }
                        else if (Request.Form["User[" + item.UserId.ToString() + "]"] == null && _contactPersonService.ExistContactPerson(contactModuleViewModel.ModuleId, item.UserId))
                        {
                            ContactPerson contactRemove = _contactPersonService.GetByModuleUser(contactModuleViewModel.ModuleId, item.UserId);
                            contactPeopleRemoveList.Add(contactRemove);
                        }
                    }
                    _contactPersonService.Add(contactPeopleAddList);
                    _contactPersonService.Delete(contactPeopleRemoveList);


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

        public ActionResult ShowUsersName(int? id)
        {
            ViewBag.ModuleId = id;
            return PartialView(_userService.Users());
        }

    }
}
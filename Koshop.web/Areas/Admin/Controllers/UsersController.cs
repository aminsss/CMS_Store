using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;
using Newtonsoft.Json;
using PagedList;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        // GET: Admin/Users
        public ActionResult Index(string SearchString = "", int page=1, int _PageSize=5)
        {
            var users = _userService.GetBySearch(1,100000,SearchString);
            
            return View(users.Records.ToPagedList(page, _PageSize));
        }
            

        [HttpGet]
        public ActionResult GetUsers(int page=1, int pageSize=5, string searchString = "")
        {
            var list = _userService.GetBySearch(page, pageSize,searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               Profile = obj.Profile,
                               moblie = obj.moblie,
                               UserId = obj.UserId,
                               RoleName = obj.Role.RoleName,
                               Name = obj.Name,
                               ISActive = obj.ISActive,
                               Email = obj.Email,
                               AddedDate = obj.AddedDate,
                           });

            return Json(new { getList ,totalCount, numPages } 
                         , JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(_roleService.Roles(), "RoleId", "RoleName");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string imagename = "no-photo.png";
                if (file != null)
                {
                    imagename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("/Upload/Profile/") + imagename);
                    //---------------------resize Images ----------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(48);
                    img.Resize(Server.MapPath("/Upload/Profile/") + imagename, Server.MapPath("/Upload/Profile/thumbnail/") + imagename);

                }
                user.Profile = imagename;
                //---------------------------
                user.ActiveCode = Guid.NewGuid().ToString().Replace("-","");
                user.AddedDate = DateTime.Now;
                _userService.Add(user);
                return RedirectToAction("Index");
            }

            ViewBag.RoleID = new SelectList(_roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        public JsonResult UniqueEmail(string Email,int UserId)
        {
            if(_userService.UniqueEmail(Email,UserId))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Uniquemoblie(string moblie, int UserId)
        {
            if (_userService.UniqueMobile(moblie, UserId))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userService.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(_roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (user.Profile != "no-photo.png")
                    {
                        if (System.IO.File.Exists(Server.MapPath("/Upload/Profile/" + user.Profile)))
                            System.IO.File.Delete(Server.MapPath("/Upload/Profile/" + user.Profile));
                        if (System.IO.File.Exists(Server.MapPath("/Upload/Profile/thumbnail/" + user.Profile)))
                            System.IO.File.Delete(Server.MapPath("/Upload/Profile/thumbnail/" + user.Profile));
                    }
                    user.Profile = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("/Upload/Profile/" + user.Profile));
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(150);
                    img.Resize(Server.MapPath("/Upload/Profile/" + user.Profile), Server.MapPath("/Upload/Profile/thumbnail/" + user.Profile));
                }
                _userService.Edit(user);
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(_roleService.Roles(), "RoleId", "RoleName", user.RoleId);
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _userService.GetById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return PartialView(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            User user = _userService.GetById(id);
            if (user.Profile != "no-photo.png")
            {
                if (System.IO.File.Exists(Server.MapPath("/Upload/Profile/" + user.Profile)))
                    System.IO.File.Delete(Server.MapPath("/Upload/Profile/" + user.Profile));
                if (System.IO.File.Exists(Server.MapPath("/Upload/Profile/thumbnail/" + user.Profile)))
                    System.IO.File.Delete(Server.MapPath("/Upload/Profile/thumbnail/" + user.Profile));
            }
            _userService.Delete(user);
            return Json(true,JsonRequestBehavior.AllowGet);
        }

    }
}

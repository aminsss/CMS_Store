using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using System.Text.RegularExpressions;

namespace Koshop.web.Controllers
{
    public class HomeController : Controller
    {
        private IMenuService _menuService;
        private IUserService _userService;
        private IMessageService _messageService;
        private IProductGroupService _productGroupService;
        private IProductService _productService;

        public HomeController(IMenuService menuService,IUserService userService,IMessageService messageService,IProductGroupService productGroupService,IProductService productService)
        {
            _menuService = menuService;
            _userService = userService;
            _messageService = messageService;
            _productGroupService = productGroupService;
            _productService = productService;
        }

        [Route("{id?}")]
        // GET: Home
        public ActionResult Index(string id)
        {
            id = id ?? "Home";
            var menu = _menuService.GetByPageName(id);
            if(menu == null)
                return View("_NotFound");

            return View(menu);
        }

        public ActionResult NotFound()
        {
            return View("_NotFound");
        }

        public ActionResult _navbar()
        {
            return PartialView(_menuService.menus());
        }

        public ActionResult _groupNavbar()
        {
            return PartialView(_productGroupService.ProductGroups());
        }

        public ActionResult _groupDirectory(string id)
        {
            ViewBag.groupSelected = id;
            return PartialView(_productGroupService.ProductGroups());
        }

        public ActionResult _ProductOffer(int id)
        {
            return PartialView(_productService.GetByGroupId(id));
        }

        public JsonResult GetMessage(string name, string email, string phone_number, string msg_subject, string message)
        {
            foreach (var item in _userService.GetAllAdmin())
            {
                if (Request.Form["User[" + item.UserId.ToString() + "]"] != null)
                {
                    Message messages = new Message()
                    {
                        SenderName = name,
                        Mobile = phone_number,
                        Email = email,
                        ToUser = item.UserId,
                        AddedDate = DateTime.Now,
                        ISRead = false,
                        Type = "Contact",
                        Subject = msg_subject,
                        ContentMessage = Regex.Replace(message, @"<[^>]+>|&nbsp;", "").Trim(),
                    };
                    _messageService.Add(messages);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor,string langCode)
        {
            string vImagePath = String.Empty;
            string vMessage = String.Empty;
            string vFilePath = String.Empty;
            string vOutput = String.Empty;
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var vFileName = DateTime.Now.ToString("yyyyMMdd-HHMMssff") +
                                    Path.GetExtension(upload.FileName).ToLower();
                    var vFolderPath = Server.MapPath("/Content/Upload/CKEditor");
                    if (!Directory.Exists(vFolderPath))
                    {
                        Directory.CreateDirectory(vFolderPath);
                    }
                    vFilePath = Path.Combine(vFolderPath, vFileName);
                    upload.SaveAs(vFilePath);
                    vImagePath = Url.Content("/Content/Upload/CKEditor/" + vFileName);
                    vMessage = "تصوير با موفقيت ذخيره شد";
                }
            }
            catch
            {
                vMessage = "There was an issue uploading";
            }
            vOutput = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + vImagePath + "\", \"" + vMessage + "\");</script></body></html>";
            return Content(vOutput);
        }
    }
}
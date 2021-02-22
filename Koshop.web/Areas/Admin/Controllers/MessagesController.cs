using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Koshop.DataLayer;
using PagedList;
using System.Data.Entity;
using MyEshop.Classes;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{

    public class MessagesController : Controller
    {
        //private AppDbContext db = new AppDbContext();
        private IUserService _userService;
        private IMessageService _messageService;

        public MessagesController (IMessageService messageService,IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        // GET: Admin/Messages
        public ActionResult Index(string type ="Contact", string searchString ="", int page = 1, int _pageSize = 5)
        {
            var list = _messageService.GetMessages(type, searchString, User.Identity.Name);
            ViewBag.type = type;

            return View(list.ToPagedList(page,_pageSize));
        }


        [HttpGet]
        public ActionResult GetMessages(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = _messageService.GetBySearch(page, pageSize, searchString, User.Identity.Name);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               moblie = obj.UsersFrom.moblie,
                               Subject = obj.Subject,
                               AddedDate = obj.AddedDate,
                               Email = obj.Email,
                               ISRead = obj.ISRead,
                               MessageId = obj.MessageId,
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }

        public ActionResult MessageContent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = _messageService.GetById(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            else if (message.ISRead == false)
            {
                _messageService.Edit(message);
            }
            return View(message);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            _messageService.Delete(id);
            return Json(true,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendEmail(int? id)
        {
            if(id != null)
            {
                Message message = _messageService.GetById(id);
                ViewBag.Email = message.Email;
                ViewBag.Mobile = message.Mobile;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail(Message message)
        {
            var from = _userService.GetUserByIdentity(User.Identity.Name);
            SendEmailSMS send = new SendEmailSMS();
            message.FromUser = from.UserId;
            message.SenderName = from.Name;
            message.Type = "Email";
            message.ISRead = true;
            if (Request.Form["SendToAll"] != null)
            {
                message.Email = "ارسال به همه";
                message.Mobile = "";
                IList<string> emailList = new List<string>();

                foreach (var item in _userService.GetAllUsers())
                {
                    emailList.Add(item.Email);
                }
                send.SendEmail(emailList, message.Subject, message.ContentMessage);
            }
            else
            {
                try
                {
                    send.SendEmail(new[] { message.Email }, message.Subject, message.ContentMessage);
                }
                catch
                {
                }
            }
            _messageService.Add(message);
            return RedirectToAction("Index");
        }

        public ActionResult SendSMS(int? id)
        {
            if (id != null)
            {
                Message message = _messageService.GetById(id);
                ViewBag.Email = message.Email;
                ViewBag.Mobile = message.Mobile;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendSMS(Message message)
        {
            var from = _userService.GetUserByIdentity(User.Identity.Name);
            SendEmailSMS send = new SendEmailSMS();
            message.FromUser = from.UserId;
            message.SenderName = from.Name;
            message.Type = "SMS";
            message.ISRead = true;
            if (Request.Form["SendToAll"] != null)
            {
                message.Email = "";
                message.Mobile = "رسال به همه";
                IList<string> mobileList = new List<string>();

                foreach (var item in _userService.GetAllUsers())
                {
                    mobileList.Add(item.moblie);
                }
                send.SendSMS(mobileList, message.ContentMessage);
            }
            else
            {
                try
                {
                send.SendSMS(new[] { message.Mobile }, message.ContentMessage);
                }
                catch
                {
                }
            }
            _messageService.Add(message);
            return RedirectToAction("Index");
        }
    }
}
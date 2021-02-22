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
using PagedList;

namespace Koshop.web.Controllers
{
    public class ownshopController : Controller
    {
        private AppDbContext db = new AppDbContext();

        //[Route("{id}")]
        public ActionResult Index(string id)
        {
            var stores = db.Stores.Include(s => s.Cities).Where(u=>u.SiteName == id).FirstOrDefault();
            return View(stores);
        }

        public ActionResult _ProductList(string id, int? page)
        {
            var storesProduct = db.StoresProducts.Where(v=>v.StoreId == id && v.IsActive == true);
            storesProduct = storesProduct.OrderByDescending(p => p.ModifiedDate);
            int pageNumber = (page ?? 1);
            return PartialView("_ProductList", storesProduct.ToPagedList(pageNumber, 20));
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult followRequest(string storeid,bool status)
        {
            int userid = db.Users.Where(x => x.moblie == User.Identity.Name).FirstOrDefault().UserId;
            
            if (status == false)
            {
                StoreFollower storeFollower = new StoreFollower()
                {
                    StoreId = storeid,
                    UserId = userid,
                };
                db.StoreFollowers.Add(storeFollower);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else if(status == true)
            {
                var item = db.StoreFollowers.Where(x => x.UserId == userid && x.StoreId == storeid).FirstOrDefault();
                db.StoreFollowers.Remove(item);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SendMessage(string storeid,string subject,string Email,string Message)
        {
            int fromUser = db.Users.Where(x => x.moblie == User.Identity.Name).FirstOrDefault().UserId;
            int toUser = db.Users.Where(x => x.UserStore.Any(u => u.Store.StoreId == storeid)).FirstOrDefault().UserId;
                Message messages = new Message()
                {
                     Email = Email,
                    ToUser = toUser,
                    FromUser = fromUser,
                    AddedDate = DateTime.Now,
                    ISRead = false,
                    Subject = subject,
                    ContentMessage = Message,
                };
                db.Messages.Add(messages);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

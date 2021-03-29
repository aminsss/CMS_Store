using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;

namespace Koshop.web.Controllers
{
    public class StoresController : Controller
    {
        private AppDbContext db = new AppDbContext();

        [Route("details/explore/{id}")]
        // GET: Stores
        public ActionResult Index(string id)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stores = db.Stores.Include(s => s.StoreInfo).Where(x=> x.IsActive == true && x.StoresProduct.Any(p=>p.Product.AliasName == id && p.IsActive == true));

            if (stores == null)
            {
                return HttpNotFound();
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Mapshow/searchinarea")]
        public JsonResult searchinarea(int? id, string bottom, string top, string left, string right,int? cityid,int? stateid,string follower="1")
        {
            var storeproduct = db.StoresProducts.Where(p => p.Product.ProductId == id && p.IsActive == true && p.Store.IsActive == true && p.Store.StoreInfo.latitute != 0);
            if (stateid != null && stateid != 0)
            {
                storeproduct = storeproduct.Where(p => p.Store.Cities.State.StateId == stateid);
            }
            if (cityid != null && cityid != 0)
            {
                storeproduct = storeproduct.Where(p => p.Store.Cities.CityId == cityid);
            }
            if (bottom != null && top != null && left != null && right != null)
            {
                double rightd, leftd, topd, bottomd;
                rightd = Convert.ToDouble(right);
                leftd = Convert.ToDouble(left);
                topd = Convert.ToDouble(top);
                bottomd = Convert.ToDouble(bottom);

                storeproduct = storeproduct.Where(s => s.Store.StoreInfo.latitute < topd && s.Store.StoreInfo.latitute > bottomd
                                    && s.Store.StoreInfo.lngitute < rightd && s.Store.StoreInfo.lngitute > leftd);
            }
            if (follower != "1" && User.Identity.IsAuthenticated)
            {
                int userid = db.Users.Where(x => x.moblie == User.Identity.Name).FirstOrDefault().UserId;
                if (follower == "2")
                    storeproduct = storeproduct.Where(p => p.Store.StoreFollower.Any(x => x.UserId == userid));
                if (follower == "3")
                    storeproduct = storeproduct.Where(p => !p.Store.StoreFollower.Any(x => x.UserId == userid));
            }

            int n = 20;
            int i = 0;
            if (storeproduct.Count() <= 20) n = storeproduct.Count();
            string[,] storeInfo = new string[n, 4];
            foreach (var item in storeproduct)
            {
                storeInfo[i, 0] = item.Store.StoreInfo.lngitute.ToString();
                storeInfo[i, 1] = item.Store.StoreInfo.latitute.ToString();
                storeInfo[i, 2] = item.StoreId;
                storeInfo[i, 3] = "<img class=\"backgrstore\" src=\"http://statics-kspub.koshop.ir/StoreImage/thumbnail/" + item.Store.StoreIcon+ "\" height=\"52\" width=\"50\" style=\"float: right; margin:5px;border: 1px solid #999;\"><div style=\"padding-right:50px\"><a class=\"color-primary\" href=/" + item.Store.SiteName+">"+item.Store.StoreName+ "</a><p >" + item.Store.StoreAddress + "</p></div>";
                i += 1;
                if (i == 20) break;
            }

            return Json(storeInfo, JsonRequestBehavior.AllowGet);
        }

        [Route("Mapshow/storeslist/{id}")]
        public ActionResult storeslist(int id, string bottom, string top, string left, string right, int? cityid, int? stateid, string follower="1")
        {
            var storeproduct = db.StoresProducts.Where(p => p.Product.ProductId == id && p.IsActive == true && p.Store.IsActive == true && p.Store.StoreInfo.latitute != 0);

            if (stateid != null && stateid != 0)
            {
                storeproduct = storeproduct.Where(x => x.Store.Cities.State.StateId == stateid);
            }
            if (cityid != null && cityid != 0)
            {
                storeproduct = storeproduct.Where(y => y.Store.Cities.CityId == cityid);
            }

            if (bottom != null && top != null && left != null && right != null)
            {
                double rightd, leftd, topd, bottomd;
                rightd = Convert.ToDouble(right);
                leftd = Convert.ToDouble(left);
                topd = Convert.ToDouble(top);
                bottomd = Convert.ToDouble(bottom);

                storeproduct = storeproduct.Where(s => s.Store.StoreInfo.latitute < topd && s.Store.StoreInfo.latitute > bottomd
                                    && s.Store.StoreInfo.lngitute < rightd && s.Store.StoreInfo.lngitute > leftd);
            }

            if (follower != "1" && User.Identity.IsAuthenticated)
            {
                int userid = db.Users.Where(x => x.moblie == User.Identity.Name).FirstOrDefault().UserId;
                if (follower == "2")
                    storeproduct = storeproduct.Where(p => p.Store.StoreFollower.Any(x => x.UserId == userid));
                if (follower == "3")
                    storeproduct = storeproduct.Where(p => !p.Store.StoreFollower.Any(x => x.UserId == userid));
            }


            return PartialView(storeproduct);
        }

        [Route("test")]
        public ActionResult test()
        {
            return View();
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

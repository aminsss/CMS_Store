using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        IOrderService _orderService;
        IUserService _userService;

        public OrdersController(IOrderService orderService,IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetOrders(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = _orderService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               Name = obj.User.Name,
                               IsFinally = obj.IsFinally,
                               AddedDate = obj.AddedDate,
                               OrderId = obj.OrderId,
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(_userService.Users(), "UserId", "UserName");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,UserId,Date,IsFinally")] Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.Add(order);
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(_userService.Users(), "UserId", "UserName", order.UserId);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order =  _orderService.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(_userService.Users(), "UserId", "UserName", order.UserId);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,UserId,Date,IsFinally")] Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.Edit(order);
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(_userService.Users(), "UserId", "UserName", order.UserId);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = _orderService.GetById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return PartialView(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _orderService.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult OrderDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IList<OrderDetail> orderDetails = _orderService.GetOrderDetail(id);
            if (orderDetails == null)
            {
                return HttpNotFound();
            }
            return View(orderDetails);
        }

        public ActionResult AllDetail(string SearchString, string currentFilter, int? page, int? _PageSize)
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetAllDetail(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = _orderService.GetAllDetail(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               Name = obj.Order.User.Name,
                               ProductTitle = obj.Product.ProductTitle,
                               ProductId = obj.ProductId,
                               ProductCount = obj.ProductCount,
                               Sum = obj.Sum,
                               IsFinally = obj.Order.IsFinally,
                               AddedDate = obj.Order.AddedDate,
                               OrderId = obj.OrderId,
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }

    }
}

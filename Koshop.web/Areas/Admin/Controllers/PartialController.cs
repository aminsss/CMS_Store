using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    [ChildActionOnly]
    public class PartialController : Controller
    {
        private IUserService _userService;
        private IProductService _productService;
        private IOrderService _orderService;

        public PartialController(IUserService userService,IProductService productService,IOrderService orderService)
        {
            _userService = userService;
            _productService = productService;
            _orderService = orderService;
        }
        // GET: Admin/Partial
        public ActionResult Sidebar()
        {
            return PartialView(_userService.GetUserByIdentity(User.Identity.Name));
        }

        public ActionResult UserCount()
        {
            return PartialView(_userService.Users());
        }
        public ActionResult ProductCount()
        {
            return PartialView(_productService.Products());
        }
        public ActionResult OrderCount()
        {
            return PartialView(_orderService.Orders());
        }
        public ActionResult IncomeSum()
        {
            return PartialView(_orderService.Orders().Where(s => s.IsFinally == true));
        }

        public ActionResult UserOptionsNav()
        {
            return PartialView(_userService.GetUserByIdentity(User.Identity.Name));
        }

    }
}
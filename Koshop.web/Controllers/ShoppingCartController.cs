//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Koshop.DataLayer;
//using Koshop.DomainClasses;
//using Koshop.ViewModels;

//namespace Koshop.web.Controllers
//{
//    public class ShoppingCartController : Controller
//    {
//        AppDbContext db = new AppDbContext(); 
//        // GET: ShoppingCart
//        public ActionResult Index()
//        {
//            List<ShowShoppingCart> shopcart = new List<ShowShoppingCart>();
//            if(Session["ShoppingCart"] != null)
//            {
//                List<ShopCartItem> shop = Session["ShoppingCart"] as List<ShopCartItem>;
//                foreach (var item in shop)
//                {
//                    var product = db.Products.Find(item.ProductId);
//                    shopcart.Add(new ShowShoppingCart()
//                    {
//                        ProductCount=item.ProductCount,
//                        ProductID = item.ProductId,
//                        //productPrice = product.productPrice,
//                        ProductTitle = product.ProductTitle,
//                        //Sum = item.ProductCount * product.productPrice
//                    });
//                }
//            }
//            return View(shopcart);
//        }

//        public ActionResult CountUp(int id)
//        {
//            List<ShopCartItem> shop = Session["ShoppingCart"] as List<ShopCartItem>;
//            int index = shop.FindIndex(s => s.ProductId == id);
//            shop[index].ProductCount += 1;
//            Session["ShoppingCart"] = shop;
//            return RedirectToAction("Index");
//        }
//        public ActionResult CountDown(int id)
//        {
//            List<ShopCartItem> shop = Session["ShoppingCart"] as List<ShopCartItem>;
//            int index = shop.FindIndex(s => s.ProductId == id);
//            shop[index].ProductCount -= 1;
//            if(shop[index].ProductCount == 0)
//            {
//                shop.Remove(shop[index]); 
//            }
//            Session["ShoppingCart"] = shop;
//            return RedirectToAction("Index");
//        }

//        public ActionResult Remove(int id)
//        {
//            List<ShopCartItem> shop = Session["ShoppingCart"] as List<ShopCartItem>;
//            int index = shop.FindIndex(s => s.ProductId == id);
//            shop.Remove(shop[index]);
//            Session["ShoppingCart"] = shop;
//            return RedirectToAction("Index");
//        }

//        [Authorize]
//        public ActionResult Save()
//        {
//            List<ShopCartItem> shop = Session["ShoppingCart"] as List<ShopCartItem>;
//            int Userid = db.Users.FirstOrDefault(u => u.moblie == User.Identity.Name).UserId;
//            Order order = new Order()
//            {
//                AddedDate = DateTime.Now,
//                UserId = Userid,
//                IsFinally=false,

//            };
//            db.Orders.Add(order);
//            foreach (var item in shop)
//            {
//                var product = db.Products.Find(item.ProductId);
//                db.OrderDetails.Add(new OrderDetail()
//                {
//                    OrderId = order.OrderId,
//                    ProductCount = item.ProductCount,
//                    //ProductPrice = product.productPrice,
//                    ProductId = product.ProductId,
//                    //Sum = item.ProductCount * product.productPrice,
//                });
//                db.SaveChanges();
//            }

//            return null;
//        }
//    }
//}
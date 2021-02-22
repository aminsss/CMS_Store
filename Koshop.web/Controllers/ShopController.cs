//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using Koshop.ViewModels;

//namespace Koshop.web.Controllers
//{
//    public class ShopController : ApiController
//    {
//        // GET: api/Shop
//        public string Get()
//        {
//            int Count = 0;
//            var session = HttpContext.Current.Session;
//            List<ShopCartItem> shopcart = new List<ShopCartItem>();
//            if (session["ShoppingCart"] != null)
//            {
//                shopcart = session["ShoppingCart"] as List<ShopCartItem>;
//                Count = shopcart.Sum(s => s.ProductCount);
//            }
//                return " سبد خرید شما " + Count + " کالا ";
//        }

//        // GET: api/Shop/5
//        public string Get(int productid)
//        {
//            var session = HttpContext.Current.Session;
//            List<ShopCartItem> shopcart = new List<ShopCartItem>();
//            if(session["ShoppingCart"] != null)
//            {
//                shopcart = session["ShoppingCart"] as List<ShopCartItem>;
//                if(shopcart.Any(s=> s.ProductId == productid))
//                {
//                    int index = shopcart.FindIndex(S => S.ProductId == productid);
//                    shopcart[index].ProductCount += 1;
//                }
//                else
//                {
//                    shopcart.Add(new ShopCartItem()
//                    {
//                        ProductId = productid,
//                        ProductCount = 1
//                    });
//                }
//            }
//            else
//            {
//                shopcart.Add(new ShopCartItem()
//                {
//                    ProductId = productid,
//                    ProductCount = 1
//                });
//            }
//            session["ShoppingCart"] = shopcart;
//            return Get();
//        }

//        // POST: api/Shop
//        public void Post([FromBody]string value)
//        {

//        }

//        // PUT: api/Shop/5
//        public void Put(int id, [FromBody]string value)
//        {

//        }

//        // DELETE: api/Shop/5
//        public void Delete(int id)
//        {

//        }
//    }
//}

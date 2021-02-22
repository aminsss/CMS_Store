using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.DataLayer;
using System.Data;
using System.Data.Entity;
using PagedList;
using Koshop.DomainClasses;

namespace Koshop.web.Controllers
{
    public class ProductController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Products
        [Route("Product/search")]
        public ActionResult index(string state, string city,string SearchString, string sortOption, int? page, string SlctGrpFilt,string yadak, string filt,string yadf, string exist )
        {
            ViewBag.SearchString = SearchString;
            ViewBag.SlctGrpFilt = SlctGrpFilt;
            ViewBag.sortOption = sortOption;
            ViewBag.state = state;
            ViewBag.city = city;
            ViewBag.filt = filt;
            ViewBag.exist = exist;
            ViewBag.yadak = yadak;
            ViewBag.yadf = yadf;
            var products = db.Products.Include(p => p.ProductGroup);

            try
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    products = products.Where(s => s.ProductTitle.Contains(SearchString)
                                           || s.ProductName.Contains(SearchString));
                }
                products = products.OrderByDescending(p => p.ProductId);

                switch (sortOption)
                {
                    case "1":
                        products = products.OrderByDescending(p => p.ProductId);
                        break;
                    case "2":
                        products = products.OrderByDescending(p => p.StoresProduct.Count());
                        break;
                    case "3":
                        products = products.OrderBy(p => p.ProductName);
                        break;
                }

                if (exist == "yes")
                {
                    products = products.Where(p => p.StoresProduct.Any(x => x.IsActive == true && x.Store.IsActive == true && x.Store.StoreInfo.latitute != 0));

                    if (state != null && state != "")
                    {
                        if (state != "همه-استان-ها")
                        {
                            products = products.Include(p => p.StoresProduct).Where(x => x.StoresProduct.Any(p => p.Store.Cities.State.StateName == state.Replace("-", " ") && p.IsActive == true));
                        }
                    }

                    if (city != null && city != "همه-شهرها" && city != "")
                    {
                        products = products.Include(p => p.StoresProduct).Where(x => x.StoresProduct.Any(p => p.Store.Cities.CityName == city.Replace("-", " ") && p.IsActive == true));
                    }
                }

                if (SlctGrpFilt != null && SlctGrpFilt != "" && SlctGrpFilt != "همه-گروه-ها")
                {

                    var groupselected = db.ProductGroups.Where(y => y.GroupTitle == SlctGrpFilt.Replace("-", " ")).FirstOrDefault();
                    if (groupselected != null)
                    {
                        var childerngroups = db.ProductGroups.Where(z => z.Path.Contains(groupselected.ProductGroupId + "/"));
                        string allgroupsString = groupselected.ProductGroupId + "/";
                        foreach (var item in childerngroups)
                        {
                            allgroupsString += item.ProductGroupId + "/";
                        }
                        var AllGrpArry = allgroupsString.Split('/');
                        products = products.Where(p => AllGrpArry.Contains(p.ProductGroupId.ToString()));
                    }
                    else
                    {
                        products = products.Where(x=>x.ProductId==0);
                    }


                    if (filt != null && filt != "" && filt != "همه-گروه-ها")
                    {
                        string[] filtA = filt.Split('_');
                        for (var i = 0; i < filtA.Length; i++)
                        {
                            var eachfiltGrp = filtA[i].Split('~');
                            var eachfilt = eachfiltGrp[1].Split('*');
                            var eachfiltG = eachfiltGrp[0].Split('*').ToString();

                            products = products.Where(p => p.Product_Attribut.Any(x => eachfilt.Contains(x.AttributItem.idfilter.ToString())));

                        }
                    }
                }
                Response.Headers["Counter"] = products.Count().ToString();

            }
            catch { };

            int? _PageSize = 20;
            int pageSize = (_PageSize ?? 20);
            int pageNumber = (page ?? 1);
            return Request.IsAjaxRequest()
                           ? (ActionResult)PartialView("_ProductList", products.ToPagedList(pageNumber, pageSize))
                           : View(products.ToPagedList(pageNumber, pageSize));
        }


        Product getfiltered(string filt , Product products)
        {
            return products;
        }

        public ActionResult getstate(string id,string city)
        {
            if(id != null)
                ViewBag.selected = id.Replace("-"," ");
            ViewBag.city = city;
            return PartialView(db.States.ToList());
        }

        public ActionResult getCities(string id)
        {
             return PartialView(db.Cities.Where(x => x.State.StateName == id.Replace("-", " ")).ToList());
        }

        public ActionResult getyadak(string id)
        {
             var idforgrp = db.ProductGroups.Where(x => x.GroupTitle == id.Replace("-", " ")).FirstOrDefault();
            if (idforgrp != null)
                return PartialView(db.ProductGroups.Where(x => x.ProductGroupId == idforgrp.ProductGroupId).FirstOrDefault());
            return null;
        }

        public ActionResult ShowGroups()
        {
            return PartialView(db.ProductGroups.ToList());
        }

        public ActionResult showProductByGroup(string id)
        {
            var Group = db.ProductGroups.Where(PG=>PG.AliasName == id).FirstOrDefault();
            return View(db.Products.Where(p => p.ProductGroupId == Group.ProductGroupId).ToArray());
        }

        public ActionResult ShowProduct(string id)
        {
            return View(db.Products.Where(x=>x.AliasName == id).FirstOrDefault());
        }

        public ActionResult GroupsOfProduct(string id)
        {
            if (id != null)
                ViewBag.selected = id.Replace("-", " ");
            return PartialView(db.ProductGroups.ToList());
        }

        public ActionResult getOthFilter(string id)
        {
            return PartialView(db.AttributGrps.Where(x => x.ProductGroup.GroupTitle == id.Replace("-"," ")));
        }
    }


}
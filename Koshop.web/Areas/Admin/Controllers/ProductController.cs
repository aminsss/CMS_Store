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
using PagedList;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {

        private IProductService _productService;
        private IProductGroupService _productGroupService;
        private IAttributeGrpService _attributeGrpService;
        private IDetailItemService _detailItemService;
        private IProductAttributeService _productAttributeService;
        private IProductDetailService _productDetailService;
        private IDetailGroupService _detailGroupService;

        public ProductController(IProductService productService, IProductGroupService productGroupService,
            IAttributeGrpService attributeGrpService,IDetailItemService detailItemService,IProductAttributeService productAttributeService
            ,IProductDetailService productDetailService,IDetailGroupService detailGroupService )
        {
            _productService = productService;
            _productGroupService = productGroupService;
            _attributeGrpService = attributeGrpService;
            _detailItemService = detailItemService;
            _productAttributeService = productAttributeService;
            _productDetailService = productDetailService;
            _detailGroupService = detailGroupService;
        }
        // GET: Admin/Product
        public ActionResult Index( int page = 1 , int _pageSize = 5 , string searchString = "")
        {
            var list = _productService.GetProducts(searchString);
            return View(list.ToPagedList(page,_pageSize));
        }

        [HttpGet]
        public ActionResult GetProducts(int page = 1 , int pageSize = 5, string searchString="")
        {
            var list = _productService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               ProductImage = obj.ProductImage,
                               ProductTitle = obj.ProductTitle,
                               AddedDate = obj.AddedDate,
                               GroupTitle = obj.ProductGroup.GroupTitle,
                               ProductId = obj.ProductId
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product products = _productService.GetById(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string imagename = "no-photo.jpg";

                if (file != null)
                {
                    //--------------------Creating names and saving to Main sarver----------------------------------
                    imagename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("/Content/Upload/productImages/Images/") + imagename);

                    //---------------------resize Images -----------------------------------------------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                    img.Resize(Server.MapPath("/Content/Upload/productImages/Images/") + imagename, Server.MapPath("/Content/Upload/productImages/thumbnail/") + imagename);

                    //----------------------upload via ftp to download server --------------------------------------
                    //FtpHelper.Upload(Server.MapPath("/Content/Upload/productImages/Images/") + imagename, "ProductImages/Images/" );
                    //FtpHelper.Upload(Server.MapPath("/Content/Upload/productImages/thumbnail/") + imagename, "ProductImages/thumbnail/");

                    //-----------------------deleting from MAin server----------------------------------------------
                    //if ((System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/Images/") + imagename)))
                    //    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/Images/") + imagename);
                }

                //saving new store and the images
                return Json(new { status = "Done", src = "/Content/Upload/productImages/thumbnail/" + imagename, ImageName = imagename });
            }
            return Json(new { status = "Error" });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Uploadgallery(HttpPostedFileBase[] files)
        {
            if (files[0] != null && files.Any())
            {
                string names = "";
                foreach (HttpPostedFileBase file in files)
                {
                    //--------------------Creating names and saving to Main sarver----------------------------------
                    string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("/Content/Upload/productImages/Images/" + galleryname));

                    //---------------------resize Images -----------------------------------------------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(300);
                    img.Resize(Server.MapPath("/Content/Upload/productImages/Images/") + galleryname, Server.MapPath("/Content/Upload/productImages/thumbnail/") + galleryname);

                    //----------------------upload via ftp to download server --------------------------------------
                    //FtpHelper.Upload(Server.MapPath("/Content/Upload/productImages/Images/") + galleryname, "ProductImages/Images/");
                    //FtpHelper.Upload(Server.MapPath("/Content/Upload/productImages/thumbnail/") + galleryname, "ProductImages/thumbnail/");

                    //-----------------------deleting from MAin server----------------------------------------------
                    //if ((System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/Images/") + galleryname)))
                    //    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/Images/") + galleryname);
                    names += galleryname + ",";
                }
                //saving new store and the images
                return Json(new { status = "Done", ImagesName = names });
            }
            return Json(new { status = "Error" });
        }

        public ActionResult galleryshowpics(string allpics)
        {
            ViewBag.allpics = allpics;
            return PartialView();
        }
        
        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle");
            return View();
        }

        // POST: Admin/Product/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product products, string ProductGalleriesName, string tags)
        {
            if (ModelState.IsValid)
            {

                //------------Create Gallery Product --------------
                var Gallery = ProductGalleriesName.Split(',');
                for (int i = 0; i < Gallery.Length - 1; i++)
                {
                    products.ProductGallery.Add(new ProductGallery
                    {
                        ProductId = products.ProductId,
                        ImageName = Gallery[i],
                    });
                }
                //-------------------Tags---------------------
                if (!string.IsNullOrEmpty(tags))
                {
                    var tag = tags.Split('-');
                    foreach (string t in tag)
                    {
                        products.ProductTag.Add(new ProductTag()
                        {
                            ProductId = products.ProductId,
                            TagTitle = t.ToLowerInvariant().Trim()
                        });
                    }
                }
                //-----------------Attribute----------------------
                //for selecting attributeGroups for a product
                foreach (var atrgrp in _attributeGrpService.GetAttrGrpProductBase(products.ProductGroupId))
                {
                    if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] != null)
                    {
                        products.Product_Attribut.Add(new Product_Attribut()
                        {
                            AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]),
                            ProductId = products.ProductId,
                        });
                    }
                }

                ////-----------------Details----------------------
                foreach (var detItm in _detailItemService.GetDetItemByProduct(products))
                {
                    {
                        products.ProductDetail.Add( new ProductDetail()
                        {
                            DetailItemId = detItm.DetailItemId,
                            ProductId = products.ProductId,
                            Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()]
                        });
                    }
                }
                _productService.Add(products);
                return RedirectToAction("/edit/" + products.ProductId);
            }

            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", products.ProductGroupId);
            return View(products);
        }

        //// GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product products = _productService.GetById(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            string tags = "";
            foreach (var t in products.ProductTag)
            {
                tags += t.TagTitle + "-";
            }
            if (tags.EndsWith("-"))
            {
                tags = tags.Substring(0, tags.Length - 1);
                ViewBag.tag = tags;
            }
            return View(products);
        }

        //// POST: Admin/Product/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product products, string ProductGalleriesName, string tags)
        {
            if (ModelState.IsValid)
            {
                //------------Create Gallery Product --------------
                List<ProductGallery> productGalleries = new List<ProductGallery>();
                var Gallery = ProductGalleriesName.Split(',');
                for (int i = 0; i < Gallery.Length - 1; i++)
                {
                    productGalleries.Add(new ProductGallery
                    {
                        ProductId = products.ProductId,
                        ImageName = Gallery[i],
                    });
                }
                _productService.AddGalleries(productGalleries);

                //-------------------Tags---------------------
                _productService.DeleteTagsByProduct(products.ProductId);

                if (!string.IsNullOrEmpty(tags))
                {
                    List<ProductTag> productTags = new List<ProductTag>();

                    foreach (string t in tags.Split('-'))
                    {
                        productTags.Add(new ProductTag
                        {
                            ProductId = products.ProductId,
                            TagTitle = t.ToLowerInvariant().Trim()
                        });
                    }
                    _productService.AddTags(productTags);
                }

                //-----------------Attribute----------------------
                List<Product_Attribut> productAttribut = new List<Product_Attribut>();

                foreach (var atrgrp in _attributeGrpService.GetAttrGrpProductBase(products.ProductGroupId)) 
                {
                    {
                        //finding product attribute if that is inserted before or not
                        var find = _productAttributeService.GetProductAttribute(products.ProductId, atrgrp.AttributGrpId);

                        if (find != null)
                        {
                            if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] == "none")
                            {
                                _productAttributeService.Delete(find);
                            }
                            //if user clicked one of attribute of this grp 
                            else if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] != null)
                            {
                                find.AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]);
                                _productAttributeService.Edit(find);
                            }
                        }
                        else
                        {
                            if (Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] != null && Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()] != "none")
                            {
                                productAttribut.Add(new Product_Attribut()
                                {
                                    AttributItemId = Convert.ToInt32(Request.Form["Grp_" + atrgrp.AttributGrpId.ToString()]),
                                    ProductId = products.ProductId,
                                });
                            }
                        }
                    }
                }
                if (productAttribut != null)
                    _productAttributeService.Add(productAttribut);

                //-----------------Details----------------------
                List<ProductDetail> productDetail = new List<ProductDetail>();

                foreach (var detItm in _detailItemService.GetDetItemByProduct(products))
                {
                    var find = _productDetailService.GetProductDetail(products.ProductId, detItm.DetailItemId);
                    if (find != null)
                    {
                        find.Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()];
                        _productDetailService.Edit(find);
                    }
                    else
                    {
                        productDetail.Add( new ProductDetail
                        {
                            DetailItemId = detItm.DetailItemId,
                            ProductId = products.ProductId,
                            Value = Request.Form["detItem_" + detItm.DetailItemId.ToString()]
                        });
                    }
                }
                if (productDetail != null)
                    _productDetailService.Add(productDetail);

                _productService.Edit(products);
                ViewBag.tag = tags;
                return RedirectToAction("/edit/" + products.ProductId);
            }
            ViewBag.ProductGroupId = new SelectList(_productGroupService.ProductGroups(), "ProductGroupId", "GroupTitle", products.ProductGroupId);
            return View(products);
        }

        //// GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product products = _productService.GetById(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return PartialView(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Product product = _productService.GetById(id);

            //-------------------------delete gallery----------------------------------------

            foreach (var gallery in product.ProductGallery.ToList())
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/Images/" + gallery.ImageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/Images/" + gallery.ImageName));

                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/thumbnail/" + gallery.ImageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/thumbnail/" + gallery.ImageName));

               _productService.DeleteGallery(gallery);
            }
            //-------------------------delete tags----------------------------------------
            _productService.DeleteTagsByProduct(product.ProductId);

            //-------------------------delete Images----------------------------------------
            if (product.ProductImage != "no-photo.jpg")
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/Images/" + product.ProductImage)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/Images/" + product.ProductImage));

                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/thumbnail/" + product.ProductImage)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/thumbnail/" + product.ProductImage));
            }

            //-----------------delete attribute ---------------------------------------------
            foreach (var atrgrp in _attributeGrpService.GetAttrGrpProductBase(product.ProductGroupId))
            {
                var find = _productAttributeService.GetProductAttribute(product.ProductId, atrgrp.AttributGrpId);
                if (find != null)
                {
                    _productAttributeService.Delete(find);
                }
            }
            _productService.Delete(product);
            return Json(true,JsonRequestBehavior.AllowGet);
        }


        public Boolean deleteGalery(int id)
        {
            var gallery = _productService.GetProductGalleryById(id);
            if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/Images/" + gallery.ImageName)))
                System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/Images/" + gallery.ImageName));
            if (System.IO.File.Exists(Server.MapPath("/Content/Upload/productImages/thumbnail/" + gallery.ImageName)))
                System.IO.File.Delete(Server.MapPath("/Content/Upload/productImages/thumbnail/" + gallery.ImageName));
            _productService.DeleteGallery(gallery);

            return true;

        }

        public ActionResult AttributeSelector(int? id)
        {
            return PartialView(_attributeGrpService.GetAttrGrpProductBase(id));
        }

        public ActionResult AttributeSelectorEdit(int? id, int? grpid)
        {
            return PartialView(_attributeGrpService.GetAttrGrpProductBase(grpid));
        }

        public ActionResult DetailInputs(int? id)
        {
            return PartialView(_detailGroupService.GetByProductGroup((int)id));
        }

        public ActionResult DetailInputsEdit(int? id, int grpid)
        {
            return PartialView(_detailGroupService.GetByProductGroup(grpid));
        }

        public JsonResult UniqueAliasName(string AliasName, int? ProductId)
        {
            if (_productService.UniqueAlias(AliasName, ProductId))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

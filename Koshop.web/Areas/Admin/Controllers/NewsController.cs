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
using System.IO;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private INewsService _newsService;
        private INewsGroupService _newsGroupService;
        private IUserService _userService;

        public NewsController(INewsService newsService,INewsGroupService newsGroupService, IUserService userService)
        {
            _newsGroupService = newsGroupService;
            _newsService = newsService;
            _userService = userService;
        }
        // GET: Admin/News
        public ActionResult Index(int page = 1, int _pageSize = 5, string searchString = "")
        {
            var users = _newsService.GetAll(searchString);

            return View(users.ToPagedList(page, _pageSize));
        }

        [HttpGet]
        public ActionResult GetNews(int page = 1, int pageSize = 5, string searchString = "")
        {
            var list = _newsService.GetBySearch(page, pageSize, searchString);

            int totalCount = list.TotalCount;
            int numPages = (int)Math.Ceiling((float)totalCount / pageSize);


            var getList = (from obj in list.Records
                           select new
                           {
                               NewsImage = obj.NewsImage,
                               NewsTitle = obj.NewsTitle,
                               AddedDate = obj.AddedDate,
                               GroupTitle = obj.NewsGroup.GroupTitle,
                               NewsId = obj.NewsId
                           });

            return Json(new { getList, totalCount, numPages }
                         , JsonRequestBehavior.AllowGet);
        }


        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsService.GetById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            ViewBag.NewsGroupId = new SelectList(_newsGroupService.NewsGroups(), "NewsGroupId", "GroupTitle");
            return View();
        }


        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news, HttpPostedFileBase NewsImage, HttpPostedFileBase[] newsGalleryPost, string tags)
        {
            if (ModelState.IsValid)
            {
                news.UserId = _userService.GetUserByIdentity(User.Identity.Name).UserId;
                news.NewsImage = "no-photo.jpg";
                if (NewsImage != null)
                {
                    news.NewsImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(NewsImage.FileName);
                    NewsImage.SaveAs(Server.MapPath("/Content/Upload/NewsImages/Images/") + news.NewsImage);
                    //---------------------resize Images ----------------------
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(150);
                    img.Resize(Server.MapPath("/Content/Upload/NewsImages/Images/") + news.NewsImage, Server.MapPath("/Content/Upload/NewsImages/thumbnail/") + news.NewsImage);
                }

                //------------Create Gallery Product --------------
                if (newsGalleryPost[0] != null && newsGalleryPost.Any())
                {
                    foreach (HttpPostedFileBase file in newsGalleryPost)
                    {
                        string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("/Content/Upload/NewsImages/Images/" + galleryname));
                        //---------------------resize Images  for Gallery----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                        img.Resize(Server.MapPath("/Content/Upload/NewsImages/Images/") + galleryname, Server.MapPath("/Content/Upload/NewsImages/thumbnail/") + galleryname);
                        news.NewsGallery.Add(new NewsGallery()
                        {
                            NewsId = news.NewsId,
                            ImageName = galleryname
                        });
                    }
                }

                //-------------------Tags---------------------
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split('-');
                    foreach (string t in tag)
                    {
                        news.NewsTag.Add(new NewsTag()
                        {
                            NewsId = news.NewsId,
                            TagsTitle = t.Trim()
                        });
                    }
                }
                _newsService.Add(news);
                return RedirectToAction("Index");
            }

            ViewBag.NewsGroupId = new SelectList(_newsGroupService.NewsGroups(), "NewsGroupId", "GroupTitle", news.NewsGroupId);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsService.GetById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.NewsGroupId = new SelectList(_newsGroupService.NewsGroups(), "NewsGroupId", "GroupTitle", news.NewsGroupId);
            string tags = "";
            foreach (var t in news.NewsTag)
            {
                tags += t.TagsTitle + "-";
            }
            if (tags.EndsWith("-"))
            {
                tags = tags.Substring(0, tags.Length - 1);
                ViewBag.tag = tags;
            }
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(News news, HttpPostedFileBase newsImages, HttpPostedFileBase[] newsGalleryPost, string tags)
        {
            if (ModelState.IsValid)
            {
                news.UserId = _userService.GetUserByIdentity(User.Identity.Name).UserId;
                if (newsImages != null)
                {
                    if (news.NewsImage != "no-photo.jpg")
                    {
                        if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage)))
                            System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage));
                        if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + news.NewsImage)))
                            System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + news.NewsImage));
                    }
                    news.NewsImage = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(newsImages.FileName);
                    newsImages.SaveAs(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage));
                    InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(150);
                    img.Resize(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage), Server.MapPath("/NewsImages/Thumbnail/" + news.NewsImage));
                }

                //------------Create Gallery Product --------------

                if (newsGalleryPost[0] != null && newsGalleryPost.Any())
                {
                    List<NewsGallery> newsGalleries = new List<NewsGallery>();

                    foreach (HttpPostedFileBase file in newsGalleryPost)
                    {
                        string galleryname = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("/Content/Upload/NewsImages/Images/" + galleryname));
                        //---------------------resize Images  for Gallery----------------------
                        InsertShowImage.ImageResizer img = new InsertShowImage.ImageResizer(350);
                        img.Resize(Server.MapPath("/Content/Upload/NewsImages/Images/") + galleryname, Server.MapPath("/NewsImages/thumbnail/") + galleryname);

                        newsGalleries.Add(new NewsGallery()
                        {
                            NewsId = news.NewsId,
                            ImageName = galleryname
                        });
                    }

                    _newsService.AddGallery(newsGalleries);
                }

                //-------------------Tags---------------------
                _newsService.DeleteTagsByNews(news.NewsId);
                if (!string.IsNullOrEmpty(tags))
                {
                    List<NewsTag> newsTag = new List<NewsTag>();

                    foreach (string tag in tags.Split('-'))
                    {
                        newsTag.Add(new NewsTag()
                        {
                            NewsId = news.NewsId,
                            TagsTitle = tag.Trim()
                        });
                    }
                    _newsService.AddTags(newsTag);
                }
                news.ModifiedDate = DateTime.Now;
                _newsService.Edit(news);
                return RedirectToAction("Index");
            }
            ViewBag.NewsGroupId = new SelectList(_newsGroupService.NewsGroups(), "NewsGroupId", "GroupsTitle", news.NewsGroupId);
            return View(news);
        }
        

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsService.GetById(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return PartialView(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            News news = _newsService.GetById(id);

            //-------------------------delete gallery----------------------------------------

            foreach (var gallery in news.NewsGallery.ToList())
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/Images/" + gallery.ImageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/Images/" + gallery.ImageName));

                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + gallery.ImageName)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + gallery.ImageName));

                _newsService.DeleteNewGalery(gallery);
            }
            //-------------------------delete tags----------------------------------------
            _newsService.DeleteTagsByNews(news.NewsId);

            //-------------------delete Images--------------------
            if (news.NewsImage != "no-photo.jpg")
            {
                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/Images/" + news.NewsImage));
                if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + news.NewsImage)))
                    System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + news.NewsImage));
            }
            _newsService.Delete(news);
            return Json(true,JsonRequestBehavior.AllowGet);
        }


        public Boolean deleteGalery(int id)
        {
            var gallery = _newsService.GetNewsGalleryById(id);
            if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/Images/" + gallery.ImageName)))
                System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/Images/" + gallery.ImageName));
            if (System.IO.File.Exists(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + gallery.ImageName)))
                System.IO.File.Delete(Server.MapPath("/Content/Upload/NewsImages/thumbnail/" + gallery.ImageName));
            _newsService.DeleteNewGalery(gallery);
            return true;

        }

        public JsonResult UniqueAliasName(string AliasName, int? NewsId)
        {
            if (_newsService.UniqueAlias(AliasName,NewsId))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}

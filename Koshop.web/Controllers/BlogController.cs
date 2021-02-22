using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.ServiceLayer;
using PagedList;
using System.Net;

namespace Koshop.web.Controllers
{
    public class BlogController : Controller
    {
        private INewsGroupService _newsGroupService;
        private INewsService _newsService;
        private IUserService _userService;
        private INewsCommentService _newsCommentService;

        public BlogController(INewsGroupService newsGroupService, INewsService newsService,IUserService userService, INewsCommentService newsCommentService) 
        {
            _newsGroupService = newsGroupService;
            _newsService = newsService;
            _userService = userService;
            _newsCommentService = newsCommentService;
        }


        public ActionResult Groups(int? id, string group, int page = 1, int pageSize = 6)
        {
            try
            {
                NewsGroup newsGroup;
                if (id != null && group == null)
                {
                    newsGroup = _newsGroupService.GetById(id);
                    group = newsGroup.AliasName;
                }
                else
                    newsGroup = _newsGroupService.GetByAlians(group);
                ViewBag.group = group;

                return View(newsGroup.News.ToPagedList(page, pageSize));
            }
            catch
            {
                return View("_NotFound");
            }
        }

        public ActionResult Contents(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = _newsService.GetByAlians(id);
            if(news == null)
            {
                return View("_NotFound");
            }
            news.Popular = ++news.Popular ?? 1;
            _newsService.Edit(news);
            return View(news);
        }

       
        public ActionResult Tags(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IList<NewsTag> newsTags = _newsService.GetTagsByName(id);
            if (newsTags == null)
            {
                return View("_NotFound");
            }
            ViewBag.tagName = id;
            return View(newsTags);
        }

        public JsonResult GetComment(int newsId, int parentId, string comment, string author, string email, string url)
        {
            string ip = AddressIpClass.GetPublicIPAddress();

            //if user is not authenticated should be allowed by time
            if (_newsCommentService.IsUserAllowed(ip) && !User.Identity.IsAuthenticated)
                return Json("NotAllowed", JsonRequestBehavior.AllowGet);

            NewsComment newsComment = new NewsComment()
            {
                Comment = comment,
                Name = author,
                Email = email,
                WebSite = url,
                ParentId = parentId,
                NewsId = newsId,
                AddedDate = DateTime.Now,
                IsActive = false,
                IP = ip,
            };
            if (User.Identity.IsAuthenticated)
            {
                User user = _userService.GetUserByIdentity(User.Identity.Name);
                newsComment.UserId = user.UserId;
                newsComment.Name = user.Name;
            }
            _newsCommentService.Add(newsComment);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
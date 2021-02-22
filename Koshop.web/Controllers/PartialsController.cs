using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.web.Controllers
{
    [ChildActionOnly]
    public class PartialsController : Controller
    {
        private IMultiPictureModuleService _multiPictureModuleService;
        private IModuleService _moduleService;

        public PartialsController(IMultiPictureModuleService multiPictureModuleService,IModuleService moduleService)
        {
            _multiPictureModuleService = multiPictureModuleService;
            _moduleService = moduleService;
        }
        public ActionResult SliderShowModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }
        public ActionResult AboutModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult ServiceModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult StoryModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult FactsModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult HowWeWorkModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult ProjectModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult TeamModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult PartnerModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult InstagramModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult HtmlModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public ActionResult NewsletterModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public ActionResult MenuModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public   ActionResult PageTitleModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        public ActionResult ContactModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public ActionResult StickySocialMediaModule(int? id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

        //public ActionResult Sidebar()
        //{
        //    return PartialView(db.Users.FirstOrDefault(u => u.moblie == User.Identity.Name));
        //}

        //public ActionResult UserCount()
        //{
        //    return PartialView(db.Users);
        //}
        //public ActionResult ProductCount()
        //{
        //    return PartialView(db.Products);
        //}
        //public ActionResult OrderCount()
        //{
        //    return PartialView(db.Orders);
        //}
        ////public ActionResult IncomeSum()
        ////{
        ////    return PartialView(db.View_packetbag.Where(s => s.IsFinally == true));
        ////}

        //[ChildActionOnly]
        //public ActionResult RetUserID()
        //{
        //    return PartialView(db.Users.Where(u => u.moblie == User.Identity.Name).FirstOrDefault());
        //}

        //public ActionResult DisMessages()
        //{
        //    return PartialView(db.Messages.Where(x=>x.ISRead == false).ToList());
        //}



        //public ActionResult mapmodule(int? id)
        //{
        //    return PartialView(db.Modules.Where(x => x.ModuleId == id).FirstOrDefault());
        //}








    }
}
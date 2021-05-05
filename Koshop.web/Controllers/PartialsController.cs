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
        public ActionResult SliderShowSmallModule(int id)
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
        public ActionResult ProductOfferModule(int id)
        {
            return PartialView(_moduleService.GetById(id));
        }
        public ActionResult StickyGroupsModule(int id)
        {
            return PartialView(_moduleService.GetById(id));
        }
        public ActionResult ProductOfferSliderModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }
        public ActionResult CategoryModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }
        public ActionResult PictureOfferModule(int id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }
        public ActionResult PageModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }
        public ActionResult MapModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }
        
        public ActionResult MenuModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public ActionResult ContactModule(int? id)
        {
            return PartialView(_moduleService.GetById(id));
        }

        public ActionResult StickySocialMediaModule(int? id)
        {
            return PartialView(_multiPictureModuleService.GetByModuleId(id));
        }

    }
}
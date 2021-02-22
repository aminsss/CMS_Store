using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;


namespace Koshop.web.Areas.Admin.Controllers
{
    public class OptionsController : Controller
    {
        private IOptionsService _optionsService;

        public OptionsController(IOptionsService optionsService)
        {
            _optionsService = optionsService;
        }

        // GET: Admin/Options
        public ActionResult Index()
        {
            //return RedirectToAction("Edit");
            IList<Options> option = _optionsService.GetOptions();
            return View(option);
        }

        [HttpPost,ActionName("Edit")]
        public ActionResult IndexPost()
        {
            List<Options> optionsList = new List<Options>();

            foreach (var item in _optionsService.GetOptions())
            {
                var option = _optionsService.GetByName(item.Name);
                if (Request.Form[item.Name.ToString()] != option.Value)
                {
                    option.Value = Request.Form[item.Name.ToString()];
                    _optionsService.Edit(option);
                }
            }
            
            return RedirectToAction("Index", new {Controller = "Default" });
        }
    }
}
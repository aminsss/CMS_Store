using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Web.Routing;
using Koshop.ServiceLayer;
using Koshop.ServiceLayer.Contracts;
using Ninject;

namespace Ninject_MVC.Controllers
{
    public class NinjectController : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectController()
        {
            ninjectKernel=new StandardKernel();
            AddBinding();
        }

        void AddBinding()
        {
            ninjectKernel.Bind<IUserService>().To<EfUserService>();
            ninjectKernel.Bind<IRoleService>().To<EfRoleService>();
            ninjectKernel.Bind<IProductService>().To<EfProductService>();
            ninjectKernel.Bind<IProductGroupService>().To<EfProductGroupService>();
            ninjectKernel.Bind<IAttributeGrpService>().To<EfAttributeGrpService>();
            ninjectKernel.Bind<IAttributeItemService>().To<EfAttributeItemService>();
            ninjectKernel.Bind<IDetailItemService>().To<EfDetailItemService>();
            ninjectKernel.Bind<IProductAttributeService>().To<EfProductAttributeService>();
            ninjectKernel.Bind<IProductDetailService>().To<EfProductDetailService>();
            ninjectKernel.Bind<IDetailGroupService>().To<EfDetailGroupService>();
            ninjectKernel.Bind<IMenuGroupService>().To<EfMenuGroupService>();
            ninjectKernel.Bind<IMenuService>().To<EfMenuService>();
            ninjectKernel.Bind<INewsGroupService>().To<EfNewsGroupService>();
            ninjectKernel.Bind<INewsService>().To<EfNewsService>();
            ninjectKernel.Bind<INewsCommentService>().To<EfNewsCommentService>();
            ninjectKernel.Bind<IMessageService>().To<EfMessageService>();
            ninjectKernel.Bind<IOrderService>().To<EfOrderService>();
            ninjectKernel.Bind<IModuleService>().To<EfModuleService>();
            ninjectKernel.Bind<IModulePageService>().To<EfModulePageService>();
            ninjectKernel.Bind<IComponentService>().To<EfComponentService>();
            ninjectKernel.Bind<IMenuModuleService>().To<EfMenuModuleService>();
            ninjectKernel.Bind<IHtmlModuleService>().To<EfHtmlModuleService>();
            ninjectKernel.Bind<IContactModuleService>().To<EfContactModuleService>();
            ninjectKernel.Bind<IContactPersonService>().To<EfContactPersonService>();
            ninjectKernel.Bind<IMultiPictureModuleService>().To<EfMultiPictureModuleService>();
            ninjectKernel.Bind<IStatisticsesService>().To<EfStatisticsesService>();
            ninjectKernel.Bind<ICountryService>().To<EfCountryService>();
            ninjectKernel.Bind<IBlockedIPsService>().To<EfBlockedIPsService>();
            ninjectKernel.Bind<IOptionsService>().To<EfOptionsService>();
        }


        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController) ninjectKernel.Get(controllerType);
        }

    }
}
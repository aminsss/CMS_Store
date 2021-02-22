using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.ServiceLayer
{
    public class EfHtmlModuleService : IHtmlModuleService
    {
        private UnitOfWork _unitOfWork;

        public EfHtmlModuleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Edit(HtmlModule htmlModule)
        {
            _unitOfWork.HtmlModuleRepository.Update(htmlModule);
            _unitOfWork.Save();
        }

        public HtmlModule GetByModuleId(int? moduleId)
        {
            return _unitOfWork.HtmlModuleRepository.Get(x => x.HtmlModuleId == moduleId).FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer
{
    public class EfMenuModuleService : IMenuModuleService
    {
        private UnitOfWork _unitOfWork;

        public EfMenuModuleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Edit(MenuModule menuModule)
        {
            _unitOfWork.MenuModuleRepository.Update(menuModule);
            _unitOfWork.Save();
        }

        public MenuModule GetByModuleId(int? id)
        {
            return _unitOfWork.MenuModuleRepository.Get(x=>x.MenuModuleId == id).FirstOrDefault();
        }

    }
}

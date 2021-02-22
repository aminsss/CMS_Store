using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    public class EfModulePageService : IModulePageService
    {
        private UnitOfWork _unitOfWork;

        public EfModulePageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Add(IList<ModulePage> modulePages)
        {
            if (modulePages.Count > 0)
            {
                foreach (var item in modulePages)
                {
                    _unitOfWork.ModulePageRepository.Insert(item);
                }
                _unitOfWork.Save();
            }
        }

        public void Delete(IList<ModulePage> modulePages)
        {
            if (modulePages.Count > 0)
            {
                foreach (var item in modulePages)
                {
                    _unitOfWork.ModulePageRepository.Delete(item);
                }
                _unitOfWork.Save();
            }
        }

        public bool ExistModulePage(int? moduleId, int? menuId)
        {
           return _unitOfWork.ModulePageRepository.Get(s => s.ModuleId == moduleId && s.MenuId == menuId).Any();
        }

        public ModulePage GetByMenuModule(int? moduleId, int? menuId)
        {
            return _unitOfWork.ModulePageRepository.Get(s => s.ModuleId == moduleId && s.MenuId == menuId).FirstOrDefault();
        }
    }
}

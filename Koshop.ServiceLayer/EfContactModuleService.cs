using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    
    public class EfContactModuleService : IContactModuleService
    {
        private UnitOfWork _unitOfWork;

        public EfContactModuleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Edit(ContactModule contactModule)
        {
            _unitOfWork.ContactModuleRepository.Update(contactModule);
            _unitOfWork.Save();
        }

        public ContactModule GetByModuleId(int? moduleId)
        {
            return _unitOfWork.ContactModuleRepository.Get(x => x.ContactModuleId == moduleId).FirstOrDefault();
        }
    }
}

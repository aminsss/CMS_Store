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
    public class EfContactPersonService : IContactPersonService
    {
        private UnitOfWork _unitOfWork;

        public EfContactPersonService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool ExistContactPerson(int? moduleId, int? userId)
        {
            return _unitOfWork.ContactPersonRepository.Get(x => x.ContactModuleId == moduleId && x.UserId == userId).Any();
        }

        public ContactPerson GetByModuleUser(int? moduleId, int? userId)
        {
            return _unitOfWork.ContactPersonRepository.Get(x => x.ContactModuleId == moduleId && x.UserId == userId).FirstOrDefault();
        }

        public void Add(IList<ContactPerson> contactPeople)
        {
            if (contactPeople.Count > 0)
            {
                foreach (var item in contactPeople)
                {
                    _unitOfWork.ContactPersonRepository.Insert(item);
                }
                _unitOfWork.Save();
            }

        }

        public void Delete(IList<ContactPerson> contactPeople)
        {
            if (contactPeople.Count > 0)
            {
                foreach (var item in contactPeople)
                {
                    _unitOfWork.ContactPersonRepository.Delete(item);
                }
                _unitOfWork.Save();
            }
        }

       
    }
}

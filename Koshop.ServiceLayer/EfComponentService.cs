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
    public class EfComponentService : IComponentService
    {
        private UnitOfWork _unitOfWork;

        public EfComponentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<Component> GetAll()
        {
            return _unitOfWork.ComponentRepository.Get().ToList();
        }
    }
}

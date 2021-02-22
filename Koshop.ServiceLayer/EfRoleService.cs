using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koshop.ServiceLayer
{
    public class EfRoleService : IRoleService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfRoleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public IEnumerable<Role> Roles()
        {
            return _unitOfWork.RoleRepository.Get();
        }
    }
}

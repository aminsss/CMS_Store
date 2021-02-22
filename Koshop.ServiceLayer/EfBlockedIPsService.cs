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
    public  class EfBlockedIPsService : IBlockedIPsService
    {
        private UnitOfWork _unitOfWork;

        public EfBlockedIPsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IList<BlockedIp> GetBlockedIPs()
        {
            return _unitOfWork.BlockedIpRepository.GetAsNoTracking().ToList();
        }

    }
}

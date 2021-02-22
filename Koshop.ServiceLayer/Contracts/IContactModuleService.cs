using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IContactModuleService
    {
        ContactModule GetByModuleId(int? moduleId);
        void Edit(ContactModule contactModule);
    }
}

using Koshop.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IRoleService
    {
        IEnumerable<Role> Roles();
    }
}

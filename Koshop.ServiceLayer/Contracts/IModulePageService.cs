using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IModulePageService
    {
        ModulePage GetByMenuModule(int? moduleId, int? menuId);
        void Add(IList<ModulePage> modulePage);
        void Delete(IList<ModulePage> modulePage);
        bool ExistModulePage(int? moduleId, int? menuId);
    }
}

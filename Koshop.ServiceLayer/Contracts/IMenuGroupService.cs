using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IMenuGroupService
    {
        DataGridViewModel<MenuGroup> GetBySearch(int? page, int? pageSize, string searchString);
        MenuGroup GetById(int? id);
        void Add(MenuGroup menuGroup);
        void Edit(MenuGroup menuGroup);
        void Delete(MenuGroup menuGroup);
        void Delete(int? id);
        IEnumerable<MenuGroup> MenuGroup();
    }
}

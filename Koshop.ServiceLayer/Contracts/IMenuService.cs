using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IMenuService
    {
        DataGridViewModel<Menu> GetByMenuGroup(int? menuGroupId);
        Menu GetById(int? id);
        IList<Menu> GetByParentId(int? parentId);
        Menu GetLastOrder(int? parentId,int? menuGroupId);
        IList<Menu> GetByParentGroupOrder(int? parentId, int? menuGroupId, int? pastDisOrder);

        Menu GetByPageName(string pageName);
        void Add(Menu menu);
        void Edit(Menu menu, int? pastDisOrder, int? pastParentId, int? pastGroupId);
        void Delete(Menu menu);
        void Delete(int id);
        bool UniquePageName(string pageName, int? menuId);
        IEnumerable<Menu> menus();
    }
}

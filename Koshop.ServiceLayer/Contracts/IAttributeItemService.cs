using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IAttributeItemService
    {
        DataGridViewModel<AttributItem> GetByAttrGrpId(int? id);
        AttributItem GetById(int? id);
        void Add(AttributItem attributItem);
        void Edit(AttributItem attributItem);
        void Delete(AttributItem attributItem);
        void Delete(int? id);
    }
}

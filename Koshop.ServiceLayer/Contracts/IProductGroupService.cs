using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IProductGroupService
    {
        DataGridViewModel<ProductGroup> GetBySearch(int? page,int? pageSize,string searchString);
        ProductGroup GetById(int? id);
        IList<ProductGroup> GetByType(string type);
        IList<ProductGroup> GetByDepth(int? depth);
        IList<ProductGroup> GetByDesDepthOrder();
        void Edit(ProductGroup productGroup);
        void Delete(ProductGroup productGroup);
        void Add(ProductGroup productGroup);
        void Delete(int? id);
        bool UniqueAlias(string aliasName, int? productGroupId);
        IEnumerable<ProductGroup> ProductGroups();
    }
}

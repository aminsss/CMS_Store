using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IDetailGroupService
    {
        DataGridViewModel<DetailGroup> GetBySearch(int page, int pageSize, string searchString);
        void Add(DetailGroup detailGroup);
        void Edit(DetailGroup detailGroup);
        void Delete(DetailGroup detailGroup);
        void Delete(int? id);
        DetailGroup GetById(int? id);
        IEnumerable<DetailGroup> DetailGroup();

        //ProductController
        IList<DetailGroup> GetByProductGroup(int productGroupId);
    }
}

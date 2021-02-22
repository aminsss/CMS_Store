using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface INewsGroupService
    {
        DataGridViewModel<NewsGroup> GetBySearch(int page, int pageSize, string searchString);
        NewsGroup GetById(int? id);
        NewsGroup GetByAlians(string alians);
        void Add(NewsGroup newsGroup);
        void Edit(NewsGroup newsGroup);
        void Delete(NewsGroup newsGroup);
        void Delete(int? id);
        bool UniqueAlias(string aliasName, int? newsGroupId);

        IEnumerable<NewsGroup> NewsGroups();

        
    }
}

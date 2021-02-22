using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IStatisticsesService
    {
        IList<Statistics> GetAll();
        IList<BrowserTableViewModel> GetByUserAgent();
        IList<OsTableViewModel> GetByUserOs();
        IList<ReferrerViewModel> GetRefreee();
        IList<PageViewViewModel> GetViewedPage();
        int GetAllCount();
        void Add(Statistics statistics);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.DataLayer;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer
{
    
    public class EfStatisticsesService : IStatisticsesService
    {

        private UnitOfWork _unitOfWork;

        public EfStatisticsesService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Statistics statistics)
        {
            _unitOfWork.StatisticsRepository.Insert(statistics);
            _unitOfWork.Save();
        }

        public IList<Statistics> GetAll()
        {
            return _unitOfWork.StatisticsRepository.Get().ToList();
        }

        public int GetAllCount()
        {
            return _unitOfWork.StatisticsRepository.Get().Count();
        }

        public IList<BrowserTableViewModel> GetByUserAgent()
        {
            var tottal = GetAllCount();
            return _unitOfWork.StatisticsRepository.Get().GroupBy(ua => new { ua.UserAgent })
                .OrderByDescending(g => g.Count()).Select(g => new BrowserTableViewModel()
                {
                    BrowserIcon = g.Key.UserAgent,
                    BrowserName = g.Key.UserAgent,
                    BrowserViewCount = g.Count(),
                    TottalVisits = tottal
                }).ToList();
        }

        public IList<OsTableViewModel> GetByUserOs()
        {
            var tottal = GetAllCount();
            return _unitOfWork.StatisticsRepository.Get().GroupBy(ua => new { ua.UserOs })
                .OrderByDescending(g => g.Count()).Select(g => new OsTableViewModel()
                {
                    OsIcon = g.Key.UserOs,
                    OsName = g.Key.UserOs,
                    OsViewCount = g.Count(),
                    TottalVisits = tottal
                }).ToList();
        }

        public IList<ReferrerViewModel> GetRefreee()
        {
            return _unitOfWork.StatisticsRepository.Get().GroupBy(
                r => new { r.Referer }).OrderByDescending(
                r => r.Count()).Select(r => new ReferrerViewModel()
                { ReferrerUrl = r.Key.Referer, ReferrerCount = r.Count() }).ToList();
        }

        public IList<PageViewViewModel> GetViewedPage()
        {
            return _unitOfWork.StatisticsRepository.Get().GroupBy(
                     r => new { r.PageViewed }).OrderByDescending(
                     r => r.Count()).Select(r => new PageViewViewModel()
                     { PageUrl = r.Key.PageViewed, PageViewCount = r.Count() }).ToList();
        }
    }
}

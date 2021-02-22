using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;

namespace Koshop.web.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        private IStatisticsesService _statisticsesService;
        private ICountryService _countryService;

        public StatisticsController(IStatisticsesService statisticsesService,ICountryService countryService)
        {
            _statisticsesService = statisticsesService;
            _countryService = countryService;
        }
        // GET: Admin/Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGeneralStatistics()
        {
            IList<Statistics> stat = new List<Statistics>();
            stat = _statisticsesService.GetAll();

            StatisticsViewModel svm = new StatisticsViewModel()
            {
                //OnlineUsers = (int)HttpContext.Application["OnlineUsersCount"],
                TodayVisits = stat.Count(ss => ss.DateStamp.Day == DateTime.Now.Day),
                TotallVisits = stat.Count,
                UniquVisitors = stat.GroupBy(ta => ta.IpAddress).Select(ta => ta.Key).Count(),

            };

            return PartialView(svm);
        }

        public ActionResult BrowserTable()
        {
            var btv = new List<BrowserTableViewModel>();
                btv.AddRange(_statisticsesService.GetByUserAgent());

            return PartialView(btv);
        }

        public ActionResult OsTable()
        {
            var otv = new List<OsTableViewModel>();
                otv.AddRange(_statisticsesService.GetByUserOs());

            return PartialView(otv);
        }

        public ActionResult CountryTable()
        {
            var countries = _countryService.GetCountries();
            return PartialView(countries);
        }

        public ActionResult Referrer()
        {
            var ur = new List<ReferrerViewModel>();
            IList<Statistics> st = new List<Statistics>();
            st = _statisticsesService.GetAll();
            foreach (var statisticse in st)
            {
                statisticse.Referer = GetHostName(statisticse.Referer);
            }
            ur.AddRange(_statisticsesService.GetRefreee());


            return PartialView(ur);
        }

        public string GetHostName(string url)
        {
            if (url != "Direct")
            {
                Uri uri = new Uri(url);
                return uri.Host;
            }
            else
            {
                return url;
            }

        }

        public ActionResult PageView()
        {
            var pv = new List<PageViewViewModel>();
            IList<Statistics> st = new List<Statistics>();
            st = _statisticsesService.GetAll();
            foreach (var statisticse in st)
            {
                statisticse.PageViewed = NormalizePageName(statisticse.PageViewed);
            }

            pv.AddRange(_statisticsesService.GetViewedPage());



            return PartialView(pv);
        }

        /// <summary>
        /// متدی برای نرمال سازی نام صفحات
        /// </summary>
        /// <param name="PageName">نام صفحه بازدید شده</param>
        /// <returns>نرمال شده نام صفحه</returns>
        public string NormalizePageName(string PageName)
        {
            if (PageName == "/")
            {
                return "Home";
            }
            else
            {
                return PageName.Remove(0, 1);
            }


        }

        public ActionResult Subdetails()
        {
            IList<Statistics> stat = new List<Statistics>();
            stat = _statisticsesService.GetAll();

            var subdetails = new SubDetailsViewModel
            {
                Today = stat.Count(d => d.DateStamp.Day == DateTime.Now.Day),
                LastDay = stat.Count(d => d.DateStamp.Day == DateTime.Now.Day - 1),
                ThisMonth = stat.Count(m => m.DateStamp.Month == DateTime.Now.Month),
                ThisYear = stat.Count(y => y.DateStamp.Year == DateTime.Now.Year),
                PeakDate = stat.GroupBy(x => x.DateStamp.ToShortDateString()).OrderByDescending(grouping => grouping.Count()).First().Key.AsDateTime(),
                LowDate = stat.GroupBy(x => x.DateStamp.ToShortDateString()).OrderByDescending(grouping => grouping.Count()).Last().Key.AsDateTime(),
            };

            //MostVisitedDate();

            return PartialView(subdetails);
        }

    }
}
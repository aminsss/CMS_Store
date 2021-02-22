using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface INewsService
    {
        DataGridViewModel<News> GetBySearch(int page, int pageSize, string searchString);
        News GetById(int? id);
        News GetByAlians(string alians);
        void Add(News news);
        void Edit(News news);
        void Delete(News news);
        void Delete(int? id);
        bool UniqueAlias(string aliasName, int? newsId);

        IEnumerable<News> GetAll(string searchString);

        //NewsTags
        void DeleteTagsByNews(int? newsId);
        void AddTags(IList<NewsTag> newsTags);

        IList<NewsTag> GetTagsByName(string tags);

        //NewsGaalery
        NewsGallery GetNewsGalleryById(int? id);
        void DeleteNewGalery(NewsGallery newsGallery);
        void AddGallery(IList<NewsGallery> newsGalleries);

       
    }
}

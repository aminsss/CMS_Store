using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer
{
    public class EfNewsService : INewsService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfNewsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       

        public DataGridViewModel<News> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<News>
            {
                Records = _unitOfWork.NewsRepository.Get(x => x.NewsTitle.Contains(searchString) || x.NewsDescription.Contains(searchString),
                x => x.OrderBy(o => o.NewsId), "NewsGroup").Skip((page - 1) * pageSize).Take(pageSize).ToList(),

                TotalCount  = _unitOfWork.NewsRepository.Get(x => x.NewsTitle.Contains(searchString) || x.NewsDescription.Contains(searchString),
                x => x.OrderBy(o => o.NewsId), "NewsGroup").Count(),

            };

            return dataGridView;
        }

        public IEnumerable<News> GetAll(string searchString)
        {
            return _unitOfWork.NewsRepository.Get(x=>x.NewsTitle.Contains(searchString) ||x.AliasName
            .Contains(searchString) ||x.NewsDescription.Contains(searchString));
        }
        public void Add(News news)
        {
            news.AddedDate = DateTime.Now;
            news.ModifiedDate = DateTime.Now;
            _unitOfWork.NewsRepository.Insert(news);
            _unitOfWork.Save();
        }

        public void Delete(News news)
        {
            _unitOfWork.NewsRepository.Delete(news);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.NewsRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Edit(News news)
        {
            _unitOfWork.NewsRepository.Update(news);
            _unitOfWork.Save();
        }

        public News GetById(int? id)
        {
           return _unitOfWork.NewsRepository.GetById(id);
        }

        public bool UniqueAlias(string aliasName, int? newsId)
        {
            return _unitOfWork.NewsRepository.Get(x => x.AliasName == aliasName && x.NewsId != newsId).Any();
        }

        public void DeleteTagsByNews(int? newsId)
        {
             _unitOfWork.NewsTagRepository.Get(t => t.NewsId == newsId).ToList()
                .ForEach(t =>_unitOfWork.NewsTagRepository.Delete(t));
            _unitOfWork.Save();
        }



        public NewsGallery GetNewsGalleryById(int? id)
        {
            return _unitOfWork.NewsGalleryRepository.GetById(id);
        }

        public void DeleteNewGalery(NewsGallery newsGallery)
        {
            _unitOfWork.NewsGalleryRepository.Delete(newsGallery);
            _unitOfWork.Save();
        }

        public void AddTags(IList<NewsTag> newsTags)
        {
            foreach (var newsTag in newsTags)
            {
                _unitOfWork.NewsTagRepository.Insert(newsTag);
            }
            _unitOfWork.Save();
        }

        public void AddGallery(IList<NewsGallery> newsGalleries)
        {
            foreach (var newsGallery in newsGalleries)
            {
                _unitOfWork.NewsGalleryRepository.Insert(newsGallery);
            }
            _unitOfWork.Save();
        }

        public News GetByAlians(string alians)
        {
            return _unitOfWork.NewsRepository.Get(x => x.AliasName == alians).FirstOrDefault();
        }

        public IList<NewsTag> GetTagsByName(string tags)
        {
            return _unitOfWork.NewsTagRepository.Get(x => x.TagsTitle == tags).ToList();
        }

       
    }
}

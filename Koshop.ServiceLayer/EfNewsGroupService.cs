using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer
{
    public class EfNewsGroupService : INewsGroupService, IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfNewsGroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<NewsGroup> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<NewsGroup>
            {
                Records = _unitOfWork.NewsGroupRepository.Get(x=>x.GroupTitle.Contains(searchString),
                x=>x.OrderBy(o=>o.NewsGroupId)).Skip((page-1) *pageSize).Take(pageSize).ToList()
            };

            return dataGridView;
        }

        public NewsGroup GetById(int? id)
        {
            return _unitOfWork.NewsGroupRepository.GetById(id);
        }

        public void Add(NewsGroup newsGroup)
        {
            newsGroup.AliasName = newsGroup.AliasName.Replace(" ", "");
            newsGroup.AddedDate = DateTime.Now;
            newsGroup.ModifiedDate = DateTime.Now;
            _unitOfWork.NewsGroupRepository.Insert(newsGroup);
            _unitOfWork.Save();
        }

        public void Edit(NewsGroup newsGroup)
        {
            //edit the children of selected Group
            EditChild(newsGroup);
            //Update the selected Group
            newsGroup.AliasName = newsGroup.AliasName.Replace(" ", "");
            newsGroup.ModifiedDate = DateTime.Now;
            _unitOfWork.NewsGroupRepository.Update(newsGroup);
            _unitOfWork.Save();
        }

        public void EditChild(NewsGroup newsGroup)
        {
            foreach (var child in  _unitOfWork.NewsGroupRepository.Get(x => x.ParentId == newsGroup.NewsGroupId))
            {
                child.Depth = newsGroup.Depth + 1;
                child.Path = newsGroup.NewsGroupId + "/" + newsGroup.Path;
                _unitOfWork.NewsGroupRepository.Update(child);
                 EditChild(child);
            }
        }

        public void Delete(NewsGroup newsGroup)
        {
            throw new NotImplementedException();
        }

        public void Delete(int? id)
        {
            var newsGroup = GetById(id);
            //First Delete children of selected Group
            RemoveChild(newsGroup);
            //Delete Selected Group
            _unitOfWork.NewsGroupRepository.Delete(newsGroup);
            _unitOfWork.Save();
        }

        public void RemoveChild(NewsGroup newsGroup)
        {
            foreach (var child in _unitOfWork.NewsGroupRepository.Get(x => x.ParentId == newsGroup.NewsGroupId))
            {
                _unitOfWork.NewsGroupRepository.Delete(child);
                RemoveChild(child);
            }
        }

        public IEnumerable<NewsGroup> NewsGroups()
        {
           return _unitOfWork.NewsGroupRepository.Get();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public bool UniqueAlias(string aliasName, int? newsGroupId)
        {
            return _unitOfWork.NewsGroupRepository.Get(s => s.AliasName == aliasName && s.NewsGroupId != newsGroupId).Any();
        }

        public NewsGroup GetByAlians(string alians)
        {
            return _unitOfWork.NewsGroupRepository.Get(x => x.AliasName == alians).FirstOrDefault();
        }

    }
}

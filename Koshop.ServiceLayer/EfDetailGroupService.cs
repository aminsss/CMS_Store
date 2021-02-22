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
    public class EfDetailGroupService : IDetailGroupService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfDetailGroupService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<DetailGroup> GetBySearch(int page, int pageSize, string searchString)
        {
            var dataGridView = new DataGridViewModel<DetailGroup>
            {
                Records = _unitOfWork.DetailGroupRepository.Get(s => s.Name.Contains(searchString),
                s => s.OrderBy(x => x.DetailGroupId), "ProductGroup")
                .Take(pageSize).Skip((page-1)*pageSize).ToList(),
            };

            return dataGridView;
        }


        public void Add(DetailGroup detailGroup)
        {
            _unitOfWork.DetailGroupRepository.Insert(detailGroup);
            _unitOfWork.Save();
        }

        public void Delete(DetailGroup detailGroup)
        {
            _unitOfWork.DetailGroupRepository.Delete(detailGroup);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.DetailGroupRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Edit(DetailGroup detailGroup)
        {
            _unitOfWork.DetailGroupRepository.Update(detailGroup);
            _unitOfWork.Save();
        }

        public DetailGroup GetById(int? id)
        {
            return _unitOfWork.DetailGroupRepository.GetById(id);
        }

        public IList<DetailGroup> GetByProductGroup(int productGroupId)
        {
            return _unitOfWork.DetailGroupRepository.Get(x => x.ProductGroupId == productGroupId).ToList();
        }
       
        public IEnumerable<DetailGroup> DetailGroup()
        {
           return _unitOfWork.DetailGroupRepository.Get();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}

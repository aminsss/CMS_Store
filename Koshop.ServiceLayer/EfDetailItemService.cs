using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    public class EfDetailItemService : IDetailItemService,IDisposable
    {
        private UnitOfWork  _unitOfWork;

        public EfDetailItemService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<DetailItem> GetByDetGrpId(int? detailGroupId)
        {
            var dataGridView = new DataGridViewModel<DetailItem>
            {
                Records = _unitOfWork.DetailItemRepository.Get(s => s.DetailGroupId == detailGroupId,
                s => s.OrderBy(x => x.DetailItemId), "DetailGroup").ToList(),
            };

            return dataGridView;

        }

        public void Add(DetailItem detailItem)
        {
            _unitOfWork.DetailItemRepository.Insert(detailItem);
            _unitOfWork.Save();
        }

        public void Delete(DetailItem detailItem)
        {
            _unitOfWork.DetailItemRepository.Delete(detailItem);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.DetailItemRepository.Delete(id);
            _unitOfWork.Save();
        }

      
        public void Edit(DetailItem detailItem)
        {
            _unitOfWork.DetailItemRepository.Update(detailItem);
            _unitOfWork.Save();
        }

        public DetailItem GetById(int? id)
        {
            return _unitOfWork.DetailItemRepository.GetById(id);
        }


        public IList<DetailItem> GetDetItemByProduct(Product product)
        {
            return _unitOfWork.DetailItemRepository.Get(x => x.DetailGroup.ProductGroupId == product.ProductGroupId).ToList();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

    }
}

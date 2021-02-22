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
    public class EfAttributeItemService : IAttributeItemService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfAttributeItemService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<AttributItem> GetByAttrGrpId(int? attributGrpId)
        {
            var DataGridView = new DataGridViewModel<AttributItem>
            {
                Records = _unitOfWork.AttributItemRepository.Get(x => x.AttributGrpId == attributGrpId,
                x => x.OrderBy(z => z.AttributItemId), "AttributGrp").ToList(),
            };

            return DataGridView;
        }

        public AttributItem GetById(int? id)
        {
            return _unitOfWork.AttributItemRepository.GetById(id);
        }

        public void Add(AttributItem attributItem)
        {
            _unitOfWork.AttributItemRepository.Insert(attributItem);
            _unitOfWork.Save();
        }

        public void Delete(AttributItem attributItem)
        {
            _unitOfWork.AttributItemRepository.Delete(attributItem);
            _unitOfWork.Save();
        }

        public void Delete(int? id)
        {
            _unitOfWork.AttributItemRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Edit(AttributItem attributItem)
        {
            _unitOfWork.AttributItemRepository.Update(attributItem);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}

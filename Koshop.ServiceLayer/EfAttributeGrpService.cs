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
    public class EfAttributeGrpService : IAttributeGrpService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfAttributeGrpService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(AttributGrp attributGrp)
        {
            _unitOfWork.AttributGrpRepository.Insert(attributGrp);
            _unitOfWork.Save();
        }

        public void Delete(AttributGrp attributGrp)
        {
            _unitOfWork.AttributGrpRepository.Delete(attributGrp);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            _unitOfWork.AttributGrpRepository.Delete(id);
            _unitOfWork.Save();
        }

       

        public void Edit(AttributGrp attributGrp)
        {
            _unitOfWork.AttributGrpRepository.Update(attributGrp);
            _unitOfWork.Save();
        }

        public IEnumerable<AttributGrp> GetAllAttributeGrp()
        {
            return _unitOfWork.AttributGrpRepository.Get();
        }

        public DataGridViewModel<AttributGrp> GetBySearch(int page, int pageSize, string searchString)
        {
            var DataGridView = new DataGridViewModel<AttributGrp>
            {
                Records = _unitOfWork.AttributGrpRepository.Get(x => x.Name.Contains(searchString), s => s.OrderBy(x => x.ProductGroupId),
                "ProductGroup").Skip((page - 1) * pageSize).Take(pageSize).ToList(),


                TotalCount = _unitOfWork.AttributGrpRepository.Get(x => x.Name.Contains(searchString), s => s.OrderBy(x => x.ProductGroupId),
                "ProductGroup").Count()
            };

            return DataGridView;
        }

        public IList<AttributGrp> GetAttrGrpProductBase(int? productGroupId)
        {
            return _unitOfWork.AttributGrpRepository.Get(x => x.ProductGroupId == productGroupId).ToList();
        }

        public AttributGrp GetById(int? id)
        {
            return _unitOfWork.AttributGrpRepository.GetById(id);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    public class EfProductAttributeService : IProductAttributeService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfProductAttributeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public Product_Attribut GetProductAttribute(int? productId, int? atrributeGrpId)
        {
           return _unitOfWork.Product_AttributRepository.Get(x => x.ProductId == productId && x.AttributItem.AttributGrp.AttributGrpId == atrributeGrpId).FirstOrDefault();
        }

        public void Delete(Product_Attribut product_Attribut)
        {
            _unitOfWork.Product_AttributRepository.Delete(product_Attribut);
        }

        public void Edit(Product_Attribut product_Attribut)
        {
            _unitOfWork.Product_AttributRepository.Update(product_Attribut);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Add(IList<Product_Attribut> product_Attributs)
        {
            foreach (var product_Attribut in product_Attributs)
            {
                _unitOfWork.Product_AttributRepository.Insert(product_Attribut);
            }
            _unitOfWork.Save();
        }
    }
}

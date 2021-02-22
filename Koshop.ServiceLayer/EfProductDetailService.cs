using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.ServiceLayer
{
    public class EfProductDetailService : IProductDetailService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfProductDetailService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ProductDetail GetProductDetail(int? productId, int? detailItemId)
        {
            return _unitOfWork.ProductDetailRepository.Get(x => x.ProductId == productId && x.DetailItemId == detailItemId).FirstOrDefault();
        }
        public void Delete(ProductDetail productDetail)
        {
            _unitOfWork.ProductDetailRepository.Delete(productDetail);
        }

        public void Edit(ProductDetail productDetail)
        {
            _unitOfWork.ProductDetailRepository.Update(productDetail);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void Add(IList<ProductDetail> productDetails)
        {
            foreach (var productDetail in productDetails)
            {
                _unitOfWork.ProductDetailRepository.Insert(productDetail);
            }
            _unitOfWork.Save();
        }
    }
}

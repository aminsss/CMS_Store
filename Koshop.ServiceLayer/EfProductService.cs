using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DataLayer;
using Koshop.DomainClasses;
using Koshop.ViewModels;
using System.Linq.Expressions;

namespace Koshop.ServiceLayer
{
    public class EfProductService : IProductService,IDisposable
    {
        private UnitOfWork _unitOfWork;

        public EfProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DataGridViewModel<Product> GetBySearch(int page, int pageSize, string searchString)
        {
            var DataGridView = new DataGridViewModel<Product>
            {
                Records = _unitOfWork.ProductRepository.Get(s=>s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString),s=>s.OrderBy(o=>o.ModifiedDate),"ProductGroup" )
                .Skip((page -1) * pageSize).Take(pageSize).ToList(),

                TotalCount = _unitOfWork.ProductRepository.Get(s => s.ProductName.Contains(searchString) ||
                s.ProductTitle.Contains(searchString), s => s.OrderBy(o => o.ModifiedDate),
                "ProductGroup").Count()
            };
            return DataGridView;
        }

        public IEnumerable<Product> GetProducts(string searchString)
        {
            return _unitOfWork.ProductRepository.Get(x => x.ProductName.Contains(searchString) 
            || x.ProductTitle.Contains(searchString) || x.AliasName.Contains(searchString));
        }

        public void Add(Product product)
        {
            product.AddedDate = DateTime.Now;
            product.ModifiedDate = DateTime.Now;
            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
        }

        public void Delete(Product product)
        {
            _unitOfWork.ProductRepository.Delete(product);
            _unitOfWork.Save();
        }

        public void Delete(int id)
        {
            _unitOfWork.ProductRepository.Delete(id);
            _unitOfWork.Save();
        }

        public void Edit(Product product)
        {
            product.ModifiedDate = DateTime.Now;
            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
        }

        public Product GetById(int? id)
        {
            return _unitOfWork.ProductRepository.GetById(id);
        }
        public bool UniqueAlias(string aliasName, int? productId)
        {
            return _unitOfWork.ProductRepository.Get(s => s.AliasName == aliasName && s.ProductId != productId).Any();
        }

        //Product Tags
        public void DeleteTagsByProduct(int? productId)
        {
            _unitOfWork.ProductTagRepository.Get(t => t.ProductId == productId).ToList().ForEach(t => _unitOfWork.ProductTagRepository.Delete(t));
        }

        //Product Gallery
        public void DeleteGallery(ProductGallery productGallery)
        {
            _unitOfWork.ProductGalleryRepository.Delete(productGallery);
            _unitOfWork.Save();
        }

        public ProductGallery GetProductGalleryById(int id)
        {
            return _unitOfWork.ProductGalleryRepository.GetById(id);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public void AddTags(IList<ProductTag> productTags)
        {
            foreach (var productTag in productTags)
            {
                _unitOfWork.ProductTagRepository.Insert(productTag);
            }
            _unitOfWork.Save();
        }

        public void AddGalleries(IList<ProductGallery> productGalleries)
        {
            foreach (var productGallery in productGalleries)
            {
                _unitOfWork.ProductGalleryRepository.Insert(productGallery);
            }
            _unitOfWork.Save();
        }

        public IEnumerable<Product> Products()
        {
            return _unitOfWork.ProductRepository.Get();
        }

        
    }
}

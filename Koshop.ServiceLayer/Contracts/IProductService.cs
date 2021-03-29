using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ViewModels;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IProductService
    {
        DataGridViewModel<Product> GetBySearch(int page,int pageSize,string searchString);
        Product GetById(int? id);
        void Add(Product product);
        void Edit(Product product);
        void Delete(Product product);
        void Delete(int id);
        bool UniqueAlias(string AliasName, int? ProductId);
        IEnumerable<Product> Products();
        IEnumerable<Product> GetProducts(string searchString);

        IEnumerable<Product> GetByGroupId(int groupId);

        //product Tags
        void DeleteTagsByProduct(int? productId);
        void AddTags(IList<ProductTag> productTags);

        //product Gallery
        void DeleteGallery(ProductGallery productGallery);
        ProductGallery GetProductGalleryById(int id);
        void AddGalleries(IList<ProductGallery> productGalleries);

       
    }
}

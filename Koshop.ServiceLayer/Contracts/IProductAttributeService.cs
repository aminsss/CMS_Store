using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.ViewModels;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IProductAttributeService
    {
        Product_Attribut GetProductAttribute(int? productId, int? atrributeGrpId);
        void Delete(Product_Attribut product_Attribut);
        void Edit(Product_Attribut product_Attribut);
        void Add(IList<Product_Attribut> product_Attributs);

    }
}

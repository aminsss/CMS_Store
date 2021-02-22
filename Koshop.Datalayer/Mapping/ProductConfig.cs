using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class ProductConfig : EntityTypeConfiguration<Product>
    {
       public ProductConfig()
        {
            // key 
            /////////// Automated


            //Propert
            //Property(t => t.MobileNo).HasMaxLength(200);

            

        }
    }
}

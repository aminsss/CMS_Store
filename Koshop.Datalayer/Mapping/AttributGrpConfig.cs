using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class AttributGrpConfig : EntityTypeConfiguration<AttributGrp>
    {
        public AttributGrpConfig()
        {
            //Property
            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.Attr_type).HasMaxLength(100);


            //Relations
            HasRequired(t => t.ProductGroup)
            .WithMany(r => r.AttributGrp)
            .HasForeignKey(f => f.ProductGroupId)
            .WillCascadeOnDelete(true);


        }
    }
}

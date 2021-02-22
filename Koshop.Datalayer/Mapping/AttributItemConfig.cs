
using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class AttributItemConfig : EntityTypeConfiguration<AttributItem>
    {
        public AttributItemConfig()
        {
            //Property
            Property(t => t.Name).HasMaxLength(100);
            Property(t => t.value).HasMaxLength(100);
            Property(t => t.idfilter).HasMaxLength(100);


            //Relations
            HasRequired(t => t.AttributGrp)
            .WithMany(r => r.AttributItem)
            .HasForeignKey(f => f.AttributGrpId)
            .WillCascadeOnDelete(true);


        }
    }
}

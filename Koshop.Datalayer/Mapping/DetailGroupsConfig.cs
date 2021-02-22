using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class DetailGroupsConfig : EntityTypeConfiguration<DetailGroup>
    {
        public DetailGroupsConfig()
        {
            //Property
            Property(t => t.Name).HasMaxLength(50);

            //Relations
            HasRequired(t => t.ProductGroup)
            .WithMany(t => t.DetailGroups)
            .HasForeignKey(t => t.ProductGroupId)
            .WillCascadeOnDelete(true);
        }
    }
}

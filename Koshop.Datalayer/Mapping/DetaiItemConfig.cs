using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class DetailItemConfig : EntityTypeConfiguration<DetailItem>
    {
        public DetailItemConfig()
        {
            //property
            Property(t => t.DetailTitle).HasMaxLength(100);
            Property(t => t.DetailType).HasMaxLength(100);


            //Relations
            HasRequired(t => t.DetailGroup)
                .WithMany(t => t.DetailItem)
                .HasForeignKey(t => t.DetailGroupId)
                .WillCascadeOnDelete(true);
        }
    }
}

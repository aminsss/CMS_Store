using System.Data.Entity.ModelConfiguration;
using Koshop.DomainClasses;


namespace Koshop.DataLayer.Mapping 
{
    public class StoreInfoConfig : EntityTypeConfiguration<StoreInfo>
    {
        public StoreInfoConfig()
        {
            //property
            Property(t => t.StoreId).HasMaxLength(50);
            Property(t => t.ZindexMap).HasMaxLength(50);
            Property(t => t.banner).HasMaxLength(50);

            //Navigation
            //this.HasRequired(t => t.Store)
            //    .WithOptional(t => t.StoreInfo)
            //    .WillCascadeOnDelete();
        }
    }
}

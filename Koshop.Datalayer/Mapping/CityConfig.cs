using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class CityConfig : EntityTypeConfiguration<City>
    {
        public CityConfig()
        {
            //Property
            Property(t => t.CityName).HasMaxLength(30);

            //Relations
            HasRequired(t => t.State)
            .WithMany(r => r.City)
            .HasForeignKey(r => new { r.StateId })
            .WillCascadeOnDelete(true);


        }
    }
}

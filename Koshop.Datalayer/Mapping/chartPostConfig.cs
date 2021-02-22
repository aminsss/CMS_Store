using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class chartPostConfig : EntityTypeConfiguration<chartPost>
    {
        public chartPostConfig()
        {
            //property
            Property(t => t.Postduty).HasMaxLength(50);
        }
    }
}

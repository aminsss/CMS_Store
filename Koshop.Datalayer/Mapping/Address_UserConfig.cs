using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class Address_UserConfig : EntityTypeConfiguration<Address_User>
    {
        public Address_UserConfig()
        {
            //Property
            Property(t => t.City).HasMaxLength(100);
            Property(t => t.HomeNo).HasMaxLength(100);
            Property(t => t.MobileNo).HasMaxLength(100);
            Property(t => t.postalCode).HasMaxLength(100);
            Property(t => t.PostAddress).HasMaxLength(100);
            Property(t => t.State).HasMaxLength(100);
            Property(t => t.NameFamily).HasMaxLength(100);

            //Relations
            HasRequired(t => t.Users)
            .WithMany(r => r.Address_Users)
            .HasForeignKey(r => new { r.UserId })
            .WillCascadeOnDelete(true);


        }
    }
}

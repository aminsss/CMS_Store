using Koshop.DomainClasses;
using System.Data.Entity.ModelConfiguration;

namespace Koshop.DataLayer.Mapping
{
    public class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            //Relations
            this.HasMany(t => t.Messages)
                .WithOptional(t => t.UsersFrom)
                .HasForeignKey(t => t.FromUser)
                .WillCascadeOnDelete(false);


            //Relations
            this.HasMany(t => t.Messages1)
                .WithOptional(t => t.UsersTo)
                .HasForeignKey(t => t.ToUser)
                .WillCascadeOnDelete(false);

            //Relations
            this.HasMany(t => t.NewsComments)
                .WithOptional(t => t.User)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}

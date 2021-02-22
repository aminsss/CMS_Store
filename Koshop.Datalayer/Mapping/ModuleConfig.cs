using System.Data.Entity.ModelConfiguration;
using Koshop.DomainClasses;

namespace Koshop.DataLayer.Mapping
{
    public class ModuleConfig : EntityTypeConfiguration<Module>
    {
        public ModuleConfig()
        {
            //Relations
            this.HasOptional(t => t.HtmlModule)
                .WithRequired(t => t.Module)
                .WillCascadeOnDelete();

            this.HasOptional(t => t.MenuModule)
                .WithRequired(t => t.Module)
                .WillCascadeOnDelete();

            this.HasOptional(t => t.ContactModule)
                .WithRequired(t => t.Module)
                .WillCascadeOnDelete();

            this.HasOptional(t => t.MultiPictureModule)
                .WithRequired(t => t.Module)
                .WillCascadeOnDelete();
        }
    }
}

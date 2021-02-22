﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Koshop.DomainClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DetailGroup
    {
        public DetailGroup()
        {
            this.DetailItem = new HashSet<DetailItem>();
        }
    
        public int DetailGroupId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Display(Name = "عنوان گروه")]
        public string Name { get; set; }

        [Display(Name = "گروه محصول")]
        public int ProductGroupId { get; set; }
    
        public virtual ProductGroup ProductGroup { get; set; }

        public virtual ICollection<DetailItem> DetailItem { get; set; }

    }
}
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
    using System.Web.Mvc;
    

    public partial class NewsGroup : TimeEntity
    {
        public NewsGroup()
        {
            this.News = new HashSet<News>();
        }
    
        public int NewsGroupId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Display(Name = "عنوان گروه")]
        public string GroupTitle { get; set; }

        public Nullable<int> Depth { get; set; }

        public string Path { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "وضعیت گروه")]
        public Nullable<bool> IsActive { get; set; }

        public Nullable<int> DisplayOrder { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "گروه والد")]
        public Nullable<int> ParentId { get; set; }

        [Display(Name = "نام مستعار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Remote("UniqueAliasName", "NewsGroup", ErrorMessage = "این نام قبلا انتخاب شده است، لطفا نام دیگری انتخاب کنید!", AdditionalFields = "NewsGroupId")]
        [StringLength(100, ErrorMessage = "تعداد کاراکترها را رعایت کنید", MinimumLength = 3)]
        public string AliasName { get; set; }
    
        public virtual ICollection<News> News { get; set; }
    }
}
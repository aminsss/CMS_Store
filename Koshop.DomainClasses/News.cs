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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;


    [DisplayName("اخبار")]
    public partial class News : TimeEntity
    {
        public News()
        {
            this.NewsGallery = new HashSet<NewsGallery>();
            this.NewsTag = new HashSet<NewsTag>();
        }
    
        public int NewsId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "گروه خبری")]
        public int NewsGroupId { get; set; }

        [Display(Name = "عنوان خبر")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="محتوا")]
        [StringLength(200, ErrorMessage = "تعداد کاراکترها را رعایت کنید",MinimumLength = 10)]
        public string NewsContent { get; set; }

        [Display(Name = "توضیحات محصول")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string NewsDescription { get; set; }

        [Display(Name = "تصویر محصول")]
        [StringLength(150, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string NewsImage { get; set; }

        public Nullable<int> Popular { get; set; }

        public int UserId { get; set; }

        [Display(Name = "نام مستعار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(150, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Remote("UniqueAliasName", "News", ErrorMessage = "این نام قبلا انتخاب شده است، لطفا نام دیگری انتخاب کنید!", AdditionalFields = "NewsId")]
        public string AliasName { get; set; }
    
        public virtual ICollection<NewsGallery> NewsGallery { get; set; }
        public virtual NewsGroup NewsGroup { get; set; }
        public virtual ICollection<NewsTag> NewsTag { get; set; }
        public virtual ICollection<NewsComment> NewsComments { get; set; }
        public virtual User User { get; set; }
    }
}

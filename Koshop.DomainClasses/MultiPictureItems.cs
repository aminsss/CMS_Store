
namespace Koshop.DomainClasses
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class MultiPictureItems
    {
        [Key]
        public int MultiPictureItemsId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "عنوان دوم")]
        public string TitleBold { get; set; }

        [Display(Name = "توضیحات")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "لینک بیشتر")]
        public string LinkMore { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }

        public int ModuleId { get; set; }

        public virtual MultiPictureModule MultiPictureModule { get; set; }
    }
}

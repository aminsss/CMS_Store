
namespace Koshop.DomainClasses
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class MultiPictureModule
    {
        public MultiPictureModule()
        {
            this.MultiPictureItems = new HashSet<MultiPictureItems>();
        }

        [Key]
        public int ModuleId { get; set; }

        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "عنوان دوم")]
        public string TitleBold { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "کاور")]
        public string Cover { get; set; }

        [Display(Name = "لینک")]
        public string Link { get; set; }

        [Display(Name = "لینک بیشتر")]
        public string LinkMore { get; set; }

        [Display(Name = "عکس")]
        public string Image { get; set; }

        public virtual Module Module { get; set; }

        public virtual ICollection<MultiPictureItems> MultiPictureItems  { get; set; }
    }
}

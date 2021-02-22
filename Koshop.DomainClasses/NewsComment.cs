using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koshop.DomainClasses
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class NewsComment : TimeEntity
    {
        public int NewsCommentId { get; set; }

        public int NewsId { get; set; }

        public Nullable<int> UserId { get; set; }

        [Display(Name ="نام")]
        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Display(Name ="ایمیل")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        [Display(Name = "وبسایت")]
        public string WebSite { get; set; }

        [Display(Name = "نظر")]
        [DataType(DataType.MultilineText)]
        public String Comment { get; set; }

        [Display(Name ="وضعیت انتشار")]
        public bool IsActive { get; set; }

        public int ParentId { get; set; }

        public virtual News News { get; set; }

        public virtual User User { get; set; }
    }
}

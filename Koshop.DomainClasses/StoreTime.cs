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

    public partial class StoreTime
    {
        public int StoreTimeId { get; set; }

        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string StoreId { get; set; }

        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string Days { get; set; }

        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string fromTime { get; set; }

        [StringLength(50, ErrorMessage = "تعداد کاراکترها را رعایت کنید")]
        public string toTime { get; set; }
    
        public virtual Store Store { get; set; }
    }
}
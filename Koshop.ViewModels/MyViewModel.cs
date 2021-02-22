using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Koshop.ViewModels
{
    public class MyViewModel
    {
        [ValidateFile(ErrorMessage = "سایز عکس باید کمتر از 1 مگابایت باشد!")]
        public HttpPostedFileBase File { get; set; }
    }
}
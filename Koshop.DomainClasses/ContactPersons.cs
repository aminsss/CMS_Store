//------------------------------------------------------------------------------
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
    
    public partial class ContactPerson
    {
        public int ContactPersonId { get; set; }

        public int ContactModuleId { get; set; }

        public int UserId { get; set; }
    
        public virtual ContactModule ContactModule { get; set; }

        public virtual User User { get; set; }
    }
}

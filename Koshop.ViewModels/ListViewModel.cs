using Koshop.DomainClasses;
using System.Collections.Generic;

namespace Koshop.ViewModels
{
    public class ListViewModelProduct
    {
        public IEnumerable<Product> Productlist { get; set; }
        public int PageIndex { get; set; }
        public int TotalItemCount { get; set; }
    }
}
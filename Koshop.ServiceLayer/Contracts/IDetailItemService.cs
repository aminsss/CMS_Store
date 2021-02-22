﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ViewModels;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IDetailItemService
    {
        DataGridViewModel<DetailItem> GetByDetGrpId(int? detailGroupId);
        DetailItem GetById(int? id);
        void Add(DetailItem detailItem);
        void Edit(DetailItem detailItem);
        void Delete(DetailItem detailItem);
        void Delete(int? id);
        IList<DetailItem> GetDetItemByProduct(Product product);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IBlockedIPsService
    {
        IList<BlockedIp> GetBlockedIPs();
    }
}

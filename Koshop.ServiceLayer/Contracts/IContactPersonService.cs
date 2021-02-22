﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface IContactPersonService
    {
        bool ExistContactPerson(int? moduleId, int? userId);
        ContactPerson GetByModuleUser(int? moduleId, int? userId);
        void Add(IList<ContactPerson> contactPeople);
        void Delete(IList<ContactPerson> contactPeople);
    }
}

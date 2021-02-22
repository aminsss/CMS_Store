using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;

namespace Koshop.ServiceLayer.Contracts
{
    public interface ICountryService
    {
        bool ExistContry(string countryCode);
        Country GetByCountryCode(string countryCode);
        IList<Country> GetCountries();
        void Edit(Country country);
        void Add(Country country);
    }
}

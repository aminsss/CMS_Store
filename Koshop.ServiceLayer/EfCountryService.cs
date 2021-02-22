using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.ServiceLayer.Contracts;
using Koshop.DomainClasses;
using Koshop.DataLayer;

namespace Koshop.ServiceLayer
{
    public class EfCountryService : ICountryService
    {
        private UnitOfWork _unitOfWork;

        public EfCountryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Country country)
        {
            _unitOfWork.CountryRepository.Insert(country);
            _unitOfWork.Save();
        }

        public void Edit(Country country)
        {
            _unitOfWork.CountryRepository.Update(country);
            _unitOfWork.Save();
        }

        public bool ExistContry(string countryCode)
        {
            return _unitOfWork.CountryRepository.Get(x => x.CountryCode.Equals(countryCode)).Any();
        }

        public Country GetByCountryCode(string countryCode)
        {
            return _unitOfWork.CountryRepository.Get(x => x.CountryCode.Equals(countryCode)).First();
        }

        public IList<Country> GetCountries()
        {
            return _unitOfWork.CountryRepository.GetAsNoTracking().ToList();
        }
    }
}

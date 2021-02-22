using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koshop.DomainClasses;
using Koshop.DataLayer;
using Koshop.ServiceLayer.Contracts;

namespace Koshop.ServiceLayer
{
    public class EfOptionsService : IOptionsService
    {
        private UnitOfWork _unitOfWork;
        
        public EfOptionsService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Edit(Options options)
        {
            _unitOfWork.OptionsRepository.Update(options);
            _unitOfWork.Save();
        }

        public Options GetByName(string name)
        {
            return _unitOfWork.OptionsRepository.Get(x => x.Name == name).FirstOrDefault() ;
        }

        public IList<Options> GetOptions()
        {
            return _unitOfWork.OptionsRepository.Get().ToList();
        }
    }
}

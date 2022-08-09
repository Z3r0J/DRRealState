using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class PropertiesTypeServices : GenericServices<SavePropertiesTypeViewModel, PropertiesTypeViewModel, PropertiesType>, IPropertiesTypeServices
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        private readonly IMapper _mapper;

        public PropertiesTypeServices(IPropertiesTypeRepository propertiesTypeRepository, IMapper mapper) : base(propertiesTypeRepository, mapper)
        {
            _propertiesTypeRepository = propertiesTypeRepository;
            _mapper = mapper;
        }
        public async Task<List<PropertiesTypeViewModel>> GetAllViewModelWithInclude()
        {
            var listSale = await _propertiesTypeRepository.GetWithIncludeAsync(new() { "Estates" });

            return _mapper.Map<List<PropertiesTypeViewModel>>(listSale);
        }
    }
}

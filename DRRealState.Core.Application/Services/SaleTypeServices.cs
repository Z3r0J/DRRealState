using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Application.Interfaces.Services;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Services
{
    public class SaleTypeServices : GenericServices<SaveSaleTypeViewModel, SaleTypeViewModel, SaleType>, ISaleTypeServices
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypeServices(ISaleTypeRepository saleTypeRepository, IMapper mapper) : base(saleTypeRepository, mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<List<SaleTypeViewModel>> GetAllViewModelWithInclude()
        {
            var listSale = await _saleTypeRepository.GetWithIncludeAsync(new() { "Estates" });

            return _mapper.Map<List<SaleTypeViewModel>>(listSale);
        }
    }
}

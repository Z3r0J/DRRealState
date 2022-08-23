using AutoMapper;
using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.SaleTypes.Querys.GetAllSaleTypeQuery
{
    public class GetAllSaleTypeQuery : IRequest<IList<SaleTypeResponse>>
    {

    }
    public class GetAllSaleTypeQueryHandler : IRequestHandler<GetAllSaleTypeQuery, IList<SaleTypeResponse>>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetAllSaleTypeQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<IList<SaleTypeResponse>> Handle(GetAllSaleTypeQuery request, CancellationToken cancellationToken)
        {
            var response = await GetAllResponse();

            return response;
        }

        private async Task<List<SaleTypeResponse>> GetAllResponse()
        {
            var saleTypeList = await _saleTypeRepository.GetAllAsync();

            if (saleTypeList.Count == 0 || saleTypeList == null) { throw new Exception($"SaleType not found."); }

            var mapper = _mapper.Map<IList<SaleTypeResponse>>(saleTypeList);

            return (List<SaleTypeResponse>)mapper;
        }
    }
}

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

namespace DRRealState.Core.Application.Features.SaleType.Querys.GetSaleTypeByIdQuery
{
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeResponse>
    {
        public int Id { get; set; }
    }
    public class GetSaleTypeByIdQueryHandler : IRequestHandler<GetSaleTypeByIdQuery, SaleTypeResponse>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public GetSaleTypeByIdQueryHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        public async Task<SaleTypeResponse> Handle(GetSaleTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetSaleTypeById(request.Id);

            return response;
        }

        private async Task<SaleTypeResponse> GetSaleTypeById(int Id)
        {

            var saleTypeList = await _saleTypeRepository.GetAllAsync();
            var saleTypeId = saleTypeList.FirstOrDefault(x => x.Id == Id);

            if (saleTypeId == null) { throw new Exception($"SaleType not found."); }

            var mapper = _mapper.Map<SaleTypeResponse>(saleTypeId);

            return mapper;
        }
    }
}

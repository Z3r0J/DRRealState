using AutoMapper;
using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.SaleTypes.Queries.GetSaleTypeByIdQuery
{
    /// <summary>
    /// Parameters to Get a Property Type By Id
    /// </summary>
    public class GetSaleTypeByIdQuery : IRequest<SaleTypeResponse>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Sale Type Id to Search on the System.")]
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

            if (saleTypeId == null) { throw new Exception($"Types for sales not found."); }

            var mapper = _mapper.Map<SaleTypeResponse>(saleTypeId);

            return mapper;
        }
    }
}

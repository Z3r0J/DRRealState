using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using DRRealState.Core.Domain.Entities;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    /// <summary>
    /// Parameters to Update a Sale Type
    /// </summary>
    public class UpdateSaleTypeCommand : IRequest<SaleTypeUpdateResponse>
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "Id of Sale Type to Update")]
        public int Id { get; set; }

        ///<example>FOR RENT</example>
        [SwaggerParameter(Description = "New Name of Sale Type to Update")]
        public string Name { get; set; }

        ///<example>Here can add Only Estates FOR RENT</example>
        [SwaggerParameter(Description = "New Description of Sale Type to Update")]
        public string Description { get; set; }
    }

    public class UpdateSaleTypeCommandHandler : IRequestHandler<UpdateSaleTypeCommand, SaleTypeUpdateResponse>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public UpdateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<SaleTypeUpdateResponse> Handle(UpdateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(command.Id);

            if (saleType == null) { throw new Exception($"Types for sales not found."); }

            saleType = _mapper.Map<SaleType>(command);

            await _saleTypeRepository.UpdateAsync(saleType, saleType.Id);

            var response = _mapper.Map<SaleTypeUpdateResponse>(saleType);

            return response;
        }
    }
}

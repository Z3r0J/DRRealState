using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace DRRealState.Core.Application.Features.SaleTypes.Commands.CreateSaleType
{
    /// <summary>
    /// Parameters to create a new Sale Type
    /// </summary>
    public class CreateSaleTypeCommand : IRequest<int>
    {
        /// <example>
        /// FOR RENT
        /// </example>
        [SwaggerParameter(Description = "Name of Sale Type")]
        public string Name { get; set; }
        /// <example>
        /// Here can add Only Estates FOR RENT
        /// </example>
        [SwaggerParameter(Description = "Description of Sale Type")]
        public string Description { get; set; }
    }

    public class CreateSaleTypeCommandHandler : IRequestHandler<CreateSaleTypeCommand, int>
    {
        private readonly ISaleTypeRepository _saleTypeRepository;
        private readonly IMapper _mapper;

        public CreateSaleTypeCommandHandler(ISaleTypeRepository saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateSaleTypeCommand command, CancellationToken cancellationToken)
        {
            var saleType = _mapper.Map<SaleType>(command);
            await _saleTypeRepository.AddAsync(saleType);

            return saleType.Id;
        }
    }
}

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

namespace DRRealState.Core.Application.Features.PropertyTypes.Commands.CreatePropertiesType
{
    public class CreatePropertiesTypeCommand : IRequest<int>
    {            
        public string Name { get; set; }        
        public string Description { get; set; }
    }
    public class CreatePropertiesTypeCommandHandler : IRequestHandler<CreatePropertiesTypeCommand, int>
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        private readonly IMapper _mapper;
        public CreatePropertiesTypeCommandHandler(IPropertiesTypeRepository propertiesTypeRepository, IMapper mapper)
        {
            _propertiesTypeRepository = propertiesTypeRepository;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreatePropertiesTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = _mapper.Map<PropertiesType>(command);
            await _propertiesTypeRepository.AddAsync(propertyType);

            return propertyType.Id;
        }
    }
}

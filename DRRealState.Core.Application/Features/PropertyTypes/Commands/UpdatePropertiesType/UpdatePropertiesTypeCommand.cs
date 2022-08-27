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

namespace DRRealState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertiesType
{
    /// <summary>
    /// Parameters to Update a Property Type
    /// </summary>
    public class UpdatePropertiesTypeCommand : IRequest<PropertyTypeUpdateResponse>
    {
        ///<example>Id= 3</example>
        [SwaggerParameter(Description = "Id of Property Type to Update")]
        public int Id { get; set; }

        ///<example>example</example>
        [SwaggerParameter(Description = "New Name of Property Type to Update")]
        public string Name { get; set; }

        ///<example>It has more Upgrade.</example>
        [SwaggerParameter(Description = "New Description of Property Type to Update")]
        public string Description { get; set; }
    }

    public class UpdatePropertiesTypeCommandHandler : IRequestHandler<UpdatePropertiesTypeCommand, PropertyTypeUpdateResponse>
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        private readonly IMapper _mapper;
        public UpdatePropertiesTypeCommandHandler(IPropertiesTypeRepository propertiesTypeRepository, IMapper mapper)
        {
            _propertiesTypeRepository = propertiesTypeRepository;
            _mapper = mapper;
        }
        public async Task<PropertyTypeUpdateResponse> Handle(UpdatePropertiesTypeCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertiesTypeRepository.GetByIdAsync(command.Id);

            if(propertyType == null) { throw new Exception($"Property Types not found."); }

            propertyType = _mapper.Map<PropertiesType>(command);

            await _propertiesTypeRepository.UpdateAsync(propertyType, propertyType.Id);

            var response = _mapper.Map<PropertyTypeUpdateResponse>(propertyType);

            return response;
        }
     
    }
}

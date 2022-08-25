using AutoMapper;
using DRRealState.Core.Application.DTOS.PropertiesType;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.PropertyTypes.Queries.GetPropertiesTypeByIdQuery
{    
     /// <summary>
     /// Parameters to Get a Property Type By Id
     /// </summary>
    public class GetPropertiesTypeByIdQuery : IRequest<PropertyTypeResponse>
    {
        /// <example>
        /// 1
        /// </example>
        [SwaggerParameter(Description = "Property Type Id to Search on the System.")]
        public int Id { get; set; }
    }

    public class GetPropertiesTypeByIdQueryHandler : IRequestHandler<GetPropertiesTypeByIdQuery, PropertyTypeResponse>
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        private readonly IMapper _mapper;
        public GetPropertiesTypeByIdQueryHandler(IPropertiesTypeRepository propertiesTypeRepository, IMapper mapper)
        {
            _propertiesTypeRepository = propertiesTypeRepository;
            _mapper = mapper;
        }
        public async Task<PropertyTypeResponse> Handle(GetPropertiesTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await GetPropertiesTypeById(request.Id);

            return response;
        }

        private async Task<PropertyTypeResponse> GetPropertiesTypeById(int Id)
        {

            var propertiesTypeList = await _propertiesTypeRepository.GetAllAsync();            
            var propertyTypeId = propertiesTypeList.FirstOrDefault(x => x.Id == Id);

            if (propertyTypeId == null) { throw new Exception($"Property Types not found."); }

            var mapper = _mapper.Map<PropertyTypeResponse>(propertyTypeId);            

            return mapper;
        }
    }
}

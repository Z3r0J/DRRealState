using AutoMapper;
using DRRealState.Core.Application.DTOS.PropertiesType;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.PropertyTypes.Queries.GetAllPropertiesTypeQuery
{
    public class GetAllPropertiesTypeQuery : IRequest<IList<PropertyTypeResponse>>
    {
    }
    public class GetAllPropertiesTypeQueryHandler : IRequestHandler<GetAllPropertiesTypeQuery, IList<PropertyTypeResponse>>
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        private readonly IMapper _mapper;

        public GetAllPropertiesTypeQueryHandler(IPropertiesTypeRepository propertiesTypeRepository, IMapper mapper)
        {
            _propertiesTypeRepository = propertiesTypeRepository;
            _mapper = mapper;
        }

        public async Task<IList<PropertyTypeResponse>> Handle(GetAllPropertiesTypeQuery request, CancellationToken cancellationToken)
        {
           var response = await GetAllResponse();

           return response;
        }

        private async Task<List<PropertyTypeResponse>> GetAllResponse()
        {
            var propertiesTypeList = await _propertiesTypeRepository.GetAllAsync();

            if (propertiesTypeList.Count == 0 || propertiesTypeList == null) { throw new Exception($"Property Types not found."); }

            var mapper = _mapper.Map<IList<PropertyTypeResponse>>(propertiesTypeList);

            return (List<PropertyTypeResponse>)mapper;
        }
    }
}

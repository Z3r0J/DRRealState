using AutoMapper;
using DRRealState.Core.Application.Interfaces.Repository;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.PropertyTypes.Commands.DeletePropertiesTypeById
{
    /// <summary>
    /// Parameters to Delete a Property Type
    /// </summary>
    public class DeletePropertiesTypeByIdCommand : IRequest<int>
    {
        ///<example>Id= 2</example>
        [SwaggerParameter(Description="Id of Property Type to delete")]
        public int Id { get; set; }
    }

    public class DeletePropertiesTypeByIdCommandHandler : IRequestHandler<DeletePropertiesTypeByIdCommand, int>
    {
        private readonly IPropertiesTypeRepository _propertiesTypeRepository;
        public DeletePropertiesTypeByIdCommandHandler(IPropertiesTypeRepository propertiesTypeRepository)
        {
            _propertiesTypeRepository = propertiesTypeRepository;            
        }
        public async Task<int> Handle(DeletePropertiesTypeByIdCommand command, CancellationToken cancellationToken)
        {
            var propertyType = await _propertiesTypeRepository.GetByIdAsync(command.Id);

            if (propertyType == null) { throw new Exception($"Property Types not found."); }

            await _propertiesTypeRepository.DeleteAsync(propertyType);

            return propertyType.Id;
        }
    }
}

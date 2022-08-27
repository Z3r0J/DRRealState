using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.PropertyTypes.Commands.UpdatePropertiesType
{
    /// <summary>
    /// Parameters to return an Updated Property Type
    /// </summary>
    public class PropertyTypeUpdateResponse
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "Id of Updated Property Type")]
        public int Id { get; set; }

        ///<example>House</example>
        [SwaggerParameter(Description = "Name of Updated Property Type")]
        public string Name { get; set; }
        ///<example>The House is black.</example>
        [SwaggerParameter(Description = "Description of Updated Property Type")]
        public string Description { get; set; }
    }
}

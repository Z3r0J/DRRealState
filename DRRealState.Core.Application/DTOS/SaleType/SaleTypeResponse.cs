using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.SaleType
{
    /// <summary>
    /// Parameters to a Sales Types.
    /// </summary>
    public class SaleTypeResponse
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "This is the Id of the Sales Types in the system.")]
        public int Id { get; set; }

        ///<example>'Direct Sale'</example>
        [SwaggerParameter(Description = "The name of the Sales Types on the system.")]
        public string Name { get; set; }

        ///<example>This type of property is the most used.</example>
        [SwaggerParameter(Description = "Description for by Sales Types in the system.")]
        public string Description { get; set; }
    }
}

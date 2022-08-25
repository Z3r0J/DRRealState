using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Features.SaleTypes.Commands.UpdateSaleType
{
    /// <summary>
    /// Parameters to return an Updated Sale Type
    /// </summary>
    public class SaleTypeUpdateResponse
    {
        [SwaggerParameter(Description = "Id of Updated Sale Type")]
        public int Id { get; set; }
        [SwaggerParameter(Description = "Name of Updated Sale Type")]
        public string Name { get; set; }
        [SwaggerParameter(Description = "Description of Updated Sale Type")]
        public string Description { get; set; }
    }
}

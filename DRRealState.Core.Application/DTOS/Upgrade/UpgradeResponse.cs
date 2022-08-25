using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Upgrade
{
    public class UpgradeResponse
    {
        ///<example>
        ///Id = 1
        ///</example>
        [SwaggerParameter(Description = "This is the Id of the Upgrade in the system.")]
        public int Id { get; set; }

        ///<example>
        ///'Direct Sale'
        ///</example>
        [SwaggerParameter(Description = "The name of the Upgrade on the system.")]
        public string Name { get; set; }

        ///<example>
        ///This type of property is the most used.
        ///</example>
        [SwaggerParameter(Description = "Description for by Upgrade in the system.")]
        public string Description { get; set; }
    }
}

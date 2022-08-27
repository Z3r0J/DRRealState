﻿using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.PropertiesType
{
    /// <summary>
    /// Parameters to a Properties Types.
    /// </summary>
    public class PropertyTypeResponse
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "This is the Id of the Properties Types in the system.")]
        public int Id { get; set; }

        ///<example>'Recidential'</example>
        [SwaggerParameter(Description = "The name of the Properties Types on the system.")]
        public string Name { get; set; }

        ///<example>This type of property is the most used.</example>
        [SwaggerParameter(Description = "Description for by Properties Types in the system.")]
        public string Description { get; set; }
    }
}

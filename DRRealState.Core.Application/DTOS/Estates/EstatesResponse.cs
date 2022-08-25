using DRRealState.Core.Application.DTOS.PropertiesType;
using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.DTOS.Upgrade;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Estates
{
    public class EstatesResponse
    {
        ///<example>
        ///Id = 1
        ///</example>
        [SwaggerParameter(Description = "This is the Id of the estates in the system.")]        
        public int Id { get; set; }

        ///<example>
        ///Code= a3ebr9
        ///</example>
        [SwaggerParameter(Description = "This is the codes of the estates in the system.")]        
        public string Code { get; set; }

        ///<example>
        ///Bathroom= 3
        ///</example>
        [SwaggerParameter(Description = "Number of bathrooms by estates in the system.")]        
        public int BathroomQuantity { get; set; }

        ///<example>
        ///Bedroom= 4
        ///</example>
        [SwaggerParameter(Description = "Number of bedroom by estates in the system.")]        
        public int BedroomQuantity { get; set; }

        ///<example>
        ///SizeInMeters = 400 m²
        ///</example>
        [SwaggerParameter(Description = "Number of meters by estes in the system.")]
        public int SizeInMeters { get; set; }

        ///<example>
        ///Price = $ 3000000
        ///</example>
        [SwaggerParameter(Description = "Price of the estates in the system.")]
        public double Price { get; set; }

        ///<example>
        ///It is a estates at a good price.
        ///</example>
        [SwaggerParameter(Description = "Description for by estates in the system.")]
        public string Description { get; set; }

        ///<example>
        ///Located in Santo Domingo.
        ///</example>
        [SwaggerParameter(Description = "Location of estates in the system.")]
        public string Ubication { get; set; }

        ///<example>
        ///PropertyTypeId = 12
        ///</example>
        [SwaggerParameter(Description = "Id of the Property Type that the estates belongs to in the system.")]
        public int PropertyTypeId { get; set; }

        ///<example>
        ///PropertiesType = Residential
        ///</example>
        [SwaggerParameter(Description = "Information on the Properties Types.")]
        public PropertyTypeResponse PropertiesType { get; set; }

        ///<example>
        ///SaleTypeId = 4
        ///</example>
        [SwaggerParameter(Description = "Id of the Sale Type that the estates belongs to in the system.")]
        public int SaleTypeId { get; set; }

        ///<example>
        ///SaleType = Direct sale
        ///</example>
        [SwaggerParameter(Description = "Information on the types of sales.")]
        public SaleTypeResponse SaleType { get; set; }

        ///<example>
        ///SizeInMeters = 400 m²
        ///</example>
        [SwaggerParameter(Description = "Information on the Upgrades.")]
        public List<UpgradeResponse> Upgrades { get; set; }

        ///<example>
        ///Id = 1
        ///</example>
        [SwaggerParameter(Description = "This is the Id of the agent in the system.")]
        public string AgentId { get; set; }

        ///<example>
        ///'Johanly'
        ///</example>
        [SwaggerParameter(Description = "The name of the agent on the system.")]
        public string AgentName { get; set; }
    }
}

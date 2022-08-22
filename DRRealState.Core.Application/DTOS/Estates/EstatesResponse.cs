using DRRealState.Core.Application.DTOS.PropertiesType;
using DRRealState.Core.Application.DTOS.SaleType;
using DRRealState.Core.Application.DTOS.Upgrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Estates
{
    public class EstatesResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int BathroomQuantity { get; set; }
        public int BedroomQuantity { get; set; }
        public int SizeInMeters { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Ubication { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyTypeResponse PropertiesType { get; set; }
        public int SaleTypeId { get; set; }
        public SaleTypeResponse SaleType { get; set; }
        public List<UpgradeResponse> Upgrades { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
    }
}

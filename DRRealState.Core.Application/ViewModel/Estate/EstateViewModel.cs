using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Application.ViewModel.Upgrade;
using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Estate
{
    public class EstateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int BathroomQuantity { get; set; }
        public int BedRoomQuantity { get; set; }
        public int SizeInMeters { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Ubication { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertiesTypeViewModel PropertiesType { get; set; }
        public int SaleTypeId { get; set; }
        public SaleTypeViewModel SaleType { get; set; }
        public List<UpgradeViewModel> Upgrade { get; set; }
        public List<Gallery> Gallery { get; set; }
        public string AgentId { get; set; }
    }
}

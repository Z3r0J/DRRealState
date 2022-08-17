using DRRealState.Core.Application.ViewModel.Gallery;
using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Application.ViewModel.Upgrade;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Estate
{
    public class SaveEstateViewModel
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
        public int SaleTypeId { get; set; }
        public List<int> UpgradeIds { get; set; }
        public string AgentId { get; set; }
        public IFormFileCollection Photos { get; set; }
        public List<GalleryViewModel> Gallery { get; set; }
        public List<SaleTypeViewModel> SaleTypes { get; set; }
        public List<PropertiesTypeViewModel> Properties { get; set; }
        public List<UpgradeViewModel> Upgrades { get; set; }
    }
}

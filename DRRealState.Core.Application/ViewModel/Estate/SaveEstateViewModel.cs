using DRRealState.Core.Application.ViewModel.Gallery;
using DRRealState.Core.Application.ViewModel.PropertiesType;
using DRRealState.Core.Application.ViewModel.SaleType;
using DRRealState.Core.Application.ViewModel.Upgrade;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Estate
{
    public class SaveEstateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int BathroomQuantity { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int BedRoomQuantity { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public int SizeInMeters { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Ubication { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Select a property type")]
        public int PropertyTypeId { get; set; }
        [Required]
        [Range(1,int.MaxValue,ErrorMessage ="Select a sale type")]
        public int SaleTypeId { get; set; }
        [Required]
        public List<int> UpgradeIds { get; set; }
        public string AgentId { get; set; }
        public IFormFileCollection Photos { get; set; }
        public List<GalleryViewModel> Gallery { get; set; }
        public List<SaleTypeViewModel> SaleTypes { get; set; }
        public List<PropertiesTypeViewModel> Properties { get; set; }
        public List<UpgradeViewModel> Upgrades { get; set; }
    }
}

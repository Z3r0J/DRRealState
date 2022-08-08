using DRRealState.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Domain.Entities
{
    public class Estate : AuditableBaseEntity
    {
        public int BathroomQuantity { get; set; }
        public int BedRoomQuantity { get; set; }
        public int SizeInMeters { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Ubication { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertiesType PropertiesType { get; set; }
        public int SaleTypeId { get; set; }
        public SaleType SaleType { get; set; }
        public ICollection<Upgrade_Estate> Upgrade { get; set; }
        public ICollection<Gallery> Gallery { get; set; }
        public ICollection<EstateFavorite> Favorites { get; set; }
        public string AgentId { get; set; }

    }
}

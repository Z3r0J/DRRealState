using DRRealState.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DRRealState.Core.Domain.Entities
{
    public class Upgrade_Estate : AuditableBaseEntity
    {
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
        public int UpgradeId { get; set; }
        public Upgrade Upgrade { get; set; }
    }
}

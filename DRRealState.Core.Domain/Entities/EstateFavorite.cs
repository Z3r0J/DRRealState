using DRRealState.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Domain.Entities
{
    public class EstateFavorite : AuditableBaseEntity
    {
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
        public string ClientId { get; set; }
    }
}

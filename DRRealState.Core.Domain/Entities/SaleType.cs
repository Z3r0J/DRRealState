﻿using DRRealState.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Domain.Entities
{
    public class SaleType : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Estate> Estates { get; set; }
    }
}

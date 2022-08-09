using DRRealState.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.PropertiesType
{
    public class PropertiesTypeViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Estate> Estates { get; set; }
    }
}

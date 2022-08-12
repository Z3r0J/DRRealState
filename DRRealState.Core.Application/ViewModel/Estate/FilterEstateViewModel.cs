using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Estate
{
    public class FilterEstateViewModel
    {
        public List<int?> EstateType { get; set; }
        public double? MinimumPrice { get; set; }
        public double? MaximumPrice { get; set; }
        public int? BathQuantity { get; set; }
        public int? BedQuantity { get; set; }
    }
}

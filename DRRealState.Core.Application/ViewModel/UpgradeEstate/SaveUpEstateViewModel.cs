using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.UpgradeEstate
{
    public class SaveUpEstateViewModel
    {
        public int EstateId { get; set; }
        public string Estate { get; set; }
        public int UpgradeId { get; set; }
        public string Upgrade { get; set; }
    }
}

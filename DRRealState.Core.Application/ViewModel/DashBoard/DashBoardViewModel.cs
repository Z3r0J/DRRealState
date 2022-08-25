using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.DashBoard
{
    public class DashBoardViewModel
    {
        public int Estates { get; set; }
        public int AgentActive { get; set; }
        public int AgentInactive { get; set; }
        public int ClientActive { get; set; }
        public int ClientInactive { get; set; }
        public int DeveloperActive { get; set; }
        public int DeveloperInactive { get; set; }
    }
}

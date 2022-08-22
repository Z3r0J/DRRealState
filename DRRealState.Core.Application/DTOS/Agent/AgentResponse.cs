using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Agent
{
    public class AgentResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int HousesQuantity { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}

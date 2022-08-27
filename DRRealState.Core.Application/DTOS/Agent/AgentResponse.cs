using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Agent
{
    /// <summary>
    /// Parameters to a new Agent
    /// </summary>
    public class AgentResponse
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "This is the Id of the agent in the system.")]       
        public string Id { get; set; }

        ///<example>'Walter'</example>
        [SwaggerParameter(Description = "The name of the agent on the system.")]        
        public string FirstName { get; set; }

        ///<example>'Casillas'</example>
        [SwaggerParameter(Description = "This is for the last name of the agent in the system.")]        
        public string LastName { get; set; }

        ///<example>Houses: 20</example>
        [SwaggerParameter(Description = "Shows the number of properties per agent in the system.")]      
        public int HousesQuantity { get; set; }

        ///<example>Phone: +1 809 900 2190</example>
        [SwaggerParameter(Description = "This is the phone number of the system agent.")]       
        public string Phone { get; set; }

        ///<example>walterC2190@email.com.do</example>
        [SwaggerParameter(Description = "Agent email in the system.")]        
        public string Email { get; set; }
    }
}

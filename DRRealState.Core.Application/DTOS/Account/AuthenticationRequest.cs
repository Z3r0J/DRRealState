using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Account
{
    public class AuthenticationRequest
    {
        ///<example>
        ///walterC2190@email.com.do
        ///</example>
        [SwaggerParameter(Description = "Request Email in the system.")]
        public string Email { get; set; }

        ///<example>
        ///New Password
        ///</example>
        [SwaggerParameter(Description = "Request Pasword in the system.")]
        public string Password { get; set; }
    }
}

using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Account
{
    /// <summary>
    /// Authentication parameters in the system.
    /// </summary>
    public class AuthenticationRequest
    {
        ///<example>example@example.com.do</example>
        [SwaggerParameter(Description = "Request Email in the system.")]
        public string Email { get; set; }

        ///<example>Insert Password</example>
        [SwaggerParameter(Description = "Request Pasword in the system.")]
        public string Password { get; set; }
    }
}

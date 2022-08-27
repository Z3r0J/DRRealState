using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Account
{
    /// <summary>
    /// Authentication parameters in the system.
    /// </summary>
    public class AuthenticationResponse
    {
        ///<example>Id= 1</example>
        [SwaggerParameter(Description = "This is the Id of the User in the system.")]
        public string Id { get; set; }

        ///<example>'Walter'</example>
        [SwaggerParameter(Description = "The name of the User on the system.")]
        public string FirstName { get; set; }

        ///<example>'Casillas'</example>
        [SwaggerParameter(Description = "This is for the last name of the User in the system.")]
        public string LastName { get; set; }

        ///<example>
        ///WalterC2190@example.com.do</example>
        [SwaggerParameter(Description = "User email in the system.")]
        public string Email { get; set; }

        ///<example>WalterC2190</example>
        [SwaggerParameter(Description = "Username of the user in the system.")]
        public string UserName { get; set; }

        ///<example>walter.jpg/png</example>
        [SwaggerParameter(Description = "Photo of the user in the system.")]
        public string PhotoUrl { get; set; }

        ///<example>402 0303003-3</example>
        [SwaggerParameter(Description = "Documents of the user in the system.")]
        public string Documents { get; set; }

        ///<example>Phone: +1 809 900 2190</example>
        [SwaggerParameter(Description = "This is the phone number by User of the system agent.")]
        public string Phone { get; set; }

        ///<example>Roles = example</example>
        [SwaggerParameter(Description = "Information on the Roles of User.")]
        public List<string> Roles { get; set; }

        ///<example>True or False</example>
        [SwaggerParameter(Description = "Verified User in the system.")]
        public bool IsVerified { get; set; }

        ///<example>eyhao2939203029302930202hakaaooeueuiwAHAHAHdjs...</example>
        [SwaggerParameter(Description = "JWT to activate you in the system.")]
        public string JWToken { get; set; }

        ///<example>Authentication Failed</example>
        [SwaggerParameter(Description = "There is an error authentication in the system.")]
        public bool HasError { get; set; }

        ///<example>Your account may be inactive.</example>
        [SwaggerParameter(Description = "Describe the error you have when authetication in the system.")]
        public string Error { get; set; }

        ///<example>Your Token is renewed</example>
        [SwaggerParameter(Description = "Renew the user's token in the system.")]
        [JsonIgnore]
        public string RefreshToken { get; set; }

    }
}

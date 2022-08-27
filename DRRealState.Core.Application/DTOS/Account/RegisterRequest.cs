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
    /// Parameters to Register a new User
    /// </summary>
    public class RegisterRequest
    {
        /// <example> Juan</example>
        [SwaggerParameter(Description = "Name of the User")]
        public string Name { get; set; }
        /// <example> Perez</example>
        [SwaggerParameter(Description = "LastName of the User")]
        public string LastName { get; set; }
        [JsonIgnore]
        public string PhotoUrl { get; set; }
        /// <example> jperez@example.com</example>
        [SwaggerParameter(Description = "Email of the User")]
        public string Email { get; set; }
        /// <example> jperez</example>
        [SwaggerParameter(Description = "Username of the User")]
        public string Username { get; set; }
        /// <example> 402-8798897-8</example>
        [SwaggerParameter(Description = "Documents of the User")]
        public string Documents { get; set; }
        /// <example> P@ssw0rd</example>
        [SwaggerParameter(Description = "Password of the User")]
        public string Password { get; set; }
        /// <example> +1 809 985 9985</example>
        [SwaggerParameter(Description = "Phone Number of the User")]
        public string Phone { get; set; }
    }
}

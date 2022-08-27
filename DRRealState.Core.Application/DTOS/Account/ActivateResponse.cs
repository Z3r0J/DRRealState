using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Account
{
    /// <summary>
    /// Parameters to Activate account.
    /// </summary>
    public class ActivateResponse
    {
        ///<example>Account Inactive</example>
        [SwaggerParameter (Description = "There is an error activating the account.")]        
        public string Error { get; set; }

        ///<example>Your account may be inactive.</example>
        [SwaggerParameter(Description = "Describe the error you have when activating the account.")]        
        public bool HasError { get; set; }
    }
}

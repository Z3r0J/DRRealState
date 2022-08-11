using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Infrastructure.Identity.Entities
{
    public class RealStateUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Documents { get; set; }
        public string Code { get; set; }
        public string PhotoUrl { get; set; }
    }
}

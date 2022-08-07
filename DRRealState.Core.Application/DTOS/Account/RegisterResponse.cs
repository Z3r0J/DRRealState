using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.DTOS.Account
{
    public class RegisterResponse
    {
        public string Error { get; set; }
        public bool HasError { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.Helpers
{
    public static class GenerateRandomCode
    {
        public static string GenerateCode() {

            Random rnd = new Random();

            return rnd.Next(0, 2147483647).ToString("D6");

        }
    }
}

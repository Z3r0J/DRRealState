using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DRRealState.Core.Application.ViewModel.Estate;

namespace DRRealState.Core.Application.ViewModel.EstateFavorite
{
    public class EstateFavoriteViewModel
    {
        public int Id { get; set; }
        public int EstateId { get; set; }
        public EstateViewModel Estate { get; set; }
        public string ClientId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.EstateFavorite
{
    public class SaveEstateFavoriteViewModel
    {
        public int Id { get; set; }
        public int EstateId { get; set; }
        public string ClientId { get; set; }
    }
}

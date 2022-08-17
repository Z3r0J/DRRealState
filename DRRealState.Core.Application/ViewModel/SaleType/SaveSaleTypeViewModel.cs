using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.SaleType
{
    public class SaveSaleTypeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
    }
}

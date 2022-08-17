using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Gallery
{
    public class SaveGalleryViewModel
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int EstateId { get; set; }
    }
}

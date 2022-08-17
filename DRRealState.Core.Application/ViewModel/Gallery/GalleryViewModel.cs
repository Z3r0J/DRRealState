using DRRealState.Core.Application.ViewModel.Estate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Application.ViewModel.Gallery
{
    public class GalleryViewModel
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int EstateId { get; set; }
        public EstateViewModel Estate { get; set; }

    }
}

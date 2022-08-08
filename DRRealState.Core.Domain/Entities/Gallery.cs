using DRRealState.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRRealState.Core.Domain.Entities
{
    public class Gallery
    {
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        //Navigation Properties
        public int EstateId { get; set; }
        public Estate Estate { get; set; }
    }
}

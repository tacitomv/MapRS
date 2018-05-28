using Mapa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mapa.Models
{
    public class POITag
    {
		public int POIId { get; set; }
		public POI POI { get; set; }
		public int TagId { get; set; }
		public Tag Tag { get; set; }
	}
}

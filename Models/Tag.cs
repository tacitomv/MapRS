using Mapa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mapa.Models
{
    public class Tag
    {
		public int Id { get; set; }
		public string Key { get; set; }
		public string Value { get; set; }
		public ICollection<POITag> POIs { get; set; }
	}
}

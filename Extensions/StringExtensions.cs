using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Mapa.Data;
using Mapa.Models;

namespace Mapa.Extensions
{
    public static class StringExtensions
    {
		internal static IEnumerable<string> GetStrings(
			IQueryable<POI> places, 
			Func<POI, bool> where,
			Func<POI, IEnumerable<string>> selectMany)
		{
			var result = places
				.Where(where)
				.SelectMany(selectMany)
				.Distinct();
			result = result.Where(x => !string.IsNullOrEmpty(x));
			return result.Select(x => x.Trim());
		}
	}
}
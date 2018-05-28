using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace Mapa.Data
{
	public static class RolesInitializer
	{
		public static async Task Initialize(RoleManager<IdentityRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync(ApplicationRoles.Administrator))
				await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Administrator));
		}
	}
}
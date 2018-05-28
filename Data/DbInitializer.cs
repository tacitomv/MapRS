using System;
using System.Threading.Tasks;
using Mapa.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Mapa.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(
			ApplicationDbContext context, 
			UserManager<User> userManager, 
			RoleManager<IdentityRole> roleManager)
		{
            context.Database.Migrate();

			await RolesInitializer.Initialize(roleManager);

			await AddAdminUser(userManager);
		}

        private async static Task AddAdminUser(UserManager<User> userManager)
        {
            var email = "contato@agstartups.org.br"; //ConfigurationManager.AppSettings["adminMail"];
            var password = "Qw121314!"; //ConfigurationManager.AppSettings["adminPassword"];

            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                await CreateUser(userManager, email, password, new string[] { ApplicationRoles.Administrator });
            }
        }

        private async static Task CreateUser(UserManager<User> userManager, string mail, string password, string[] role, string organizationId = null)
        {
            User usr = new User
            {
                UserName = mail,
                Email = mail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(usr, password);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(usr, role);
            }
            // if (!string.IsNullOrEmpty(organizationId))
            // {
            //     await userManager.AddClaimAsync(usr, new Claim("OrganizationId", organizationId));
            // }
        }
    }
}
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mapa.Data;
using Mapa.Models;
using MapaServices;
using Mapa.Extensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using AutoMapper;

namespace Mapa
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

			if (env.IsDevelopment())
			{
				// For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
				builder.AddUserSecrets<Startup>();
			}

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
			//options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

			services.AddSingleton<IFileProvider>(
				new PhysicalFileProvider(
					Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

            services.AddMvc(config =>
				{
					config.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());
				});

			Mapper.Initialize(cfg => cfg.CreateMap<POIViewModel, POI>());

			// Add application services.
			services.AddTransient<IEmailSender, AuthMessageSender>();
			services.AddTransient<ISmsSender, AuthMessageSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ILoggerFactory loggerFactory,
			RoleManager<IdentityRole> roleManager)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			var supportedCultures = new[]
			{
				new CultureInfo("en-US"),
				new CultureInfo("pt-BR"),
			};

			app.UseRequestLocalization(new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("pt-BR"),
				// Formatting numbers, dates, etc.
				SupportedCultures = supportedCultures,
				// UI strings that we have localized.
				SupportedUICultures = supportedCultures
			});

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
						name: "login-route",
						template: "login",
						defaults: new { controller = "Account", action = "Login" });
				routes.MapRoute(
						name: "manage-route",
						template: "admin",
						defaults: new { controller = "POIs", action = "Index" });
				routes.MapRoute(
						name: "shuffle-route",
						template: "portfolio",
						defaults: new { controller = "Home", action = "Shuffle" });
				routes.MapRoute(
						name: "default",
						template: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}

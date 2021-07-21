using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CompareSite
{
    public class Startup
    {
	///
	public static string ServiceAddress {get => mServiceAddress; }
	private static string mServiceAddress;

       	/// 
        public static string AdminLogin { get => mAdminLogin; }
        private static string mAdminLogin;

        /// 
        public static string AdminPassword { get => mAdminPassword; }
        private static string mAdminPassword;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });

	// 
	mServiceAddress = Configuration.GetValue<string>("ServiceAddress");
	// 
	mAdminLogin = Configuration.GetValue<string>("AdminLogin");
	// 
	mAdminPassword = Configuration.GetValue<string>("AdminPassword");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Compare/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Compare}/{action=Index}/{id?}");
            });
        }
    }
}

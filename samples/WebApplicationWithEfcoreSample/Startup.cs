using Casbin.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using WebApplicationSample.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Security.Claims;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.Adapter.EFCore;
using Casbin;

namespace WebApplicationSample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //The sample user emails are alice@example.com/bob@example.com and password is Pass123$
            services.AddDbContext<CasbinDbContext<string>>(options =>
                options.UseSqlite("Data Source=AdapterSample.db"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=UserSample.db"));
            services.AddCasbinAuthorization(options =>
            {
                options.PreferSubClaimType = ClaimTypes.Name;
                options.DefaultModelPath = Path.Combine("CasbinConfigs", "basic_model.conf");
                //options.DefaultPolicyPath = Path.Combine("CasbinConfigs", "basic_policy.csv");

                options.DefaultEnforcerFactory = (p, m) =>
                    new Enforcer(m, new EFCoreAdapter<string>(p.GetRequiredService<CasbinDbContext<string>>()));
                options.DefaultRequestTransformerType = typeof(BasicRequestTransformer);
            });
            services.AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            //If you
            app.UseAuthentication();


            app.UseCasbinAuthorization();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

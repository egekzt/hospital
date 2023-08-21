using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using YourProject.Models;
using Microsoft.AspNetCore.Identity;
namespace hospital
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            _appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddHttpContextAccessor();
            //services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddScoped<IhospitalContext,hospitalContext>(options => options.UseNpgsql(_appSettings.ConnectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<hospital_context>()
            .AddDefaultTokenProviders();
            services.AddHealthChecks()
                .AddDbContextCheck<hospitalContext>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                 {
                     options.LoginPath = "/Login/Index"; // Set the login path
                                                         // Additional options...
                 });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DoctorOnly", policy =>
                    policy.RequireRole("Doctor"));
                options.AddPolicy("SecretaryOnly", policy =>
                    policy.RequireRole("Secretary"));
                options.AddPolicy("PatientOnly", policy =>
                    policy.RequireRole("Patient"));
                    
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseHealthChecks("/health"); // Endpoint to check the health status

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Login}/{action=Index}");
            });
        }
    }
}

using GibiSu.Data;
using GibiSu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GibiSu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ApplicationDbContext context;

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Pages}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "pages",
                pattern: "/{*id}",
                defaults: new { controller = "Pages", action = "Details" });
            app.MapRazorPages();

            context = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider.GetService <ApplicationDbContext> ();
            context.Database.Migrate(); 
            Models.Page homePage = new Models.Page();
            if (homePage.Url != "index")
            {
                homePage.Url = "index";
                homePage.Title = "Ana Sayfa";
                context.Add(homePage);
                context.SaveChangesAsync();
            }
            

            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("tr-TR");
            System.Globalization.CultureInfo.CurrentCulture = culture;
            System.Globalization.CultureInfo.CurrentUICulture = culture;


            app.Run();
        }
    }
}
using GibiSu.Data;
using GibiSu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.ConfigureApplicationCookie(options => options.LoginPath = new PathString("/Users/Login"));
            builder.Services.AddRazorPages();

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
                name: "products",
                pattern: "/urunlerimiz",
                defaults: new { controller = "Products", action = "Index" });
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "orders",
                pattern: "/siparislerim",
                defaults: new { controller = "Orders", action = "Index" });
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "orderProducts",
                pattern: "/sepetim",
                defaults: new { controller = "OrderProducts", action = "Index" });
            app.MapRazorPages();

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
            
            Models.Menu menu = context.Menus.Where(m => m.Name == "footer").FirstOrDefault();
            if (menu == null)
            {
                menu=new Menu();
                menu.Name = "footer";
                context.Add(menu);
                context.SaveChanges();
            }
            Models.Page homePage = context.Pages.Where(m => m.Url == "index").FirstOrDefault();
           
            if (homePage == null)
            {
                homePage=new Page();
                homePage.Order = 100;
                homePage.Url = "index";
                homePage.Title = "Ana Sayfa";
                context.Add(homePage);
                context.SaveChanges();
            }

            


            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("tr-TR");
            System.Globalization.CultureInfo.CurrentCulture = culture;
            System.Globalization.CultureInfo.CurrentUICulture = culture;


            app.Run();
        }
    }
}
using GiftStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftStoreMVC;
public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddDbContext<ModelContext>(options => options.UseOracle(
            builder.Configuration.GetConnectionString("GiftStoreMVCConnection")
        ));
        builder.Services.AddSession(options => options.IOTimeout = TimeSpan.FromMinutes(60));

        WebApplication app = builder.Build();

        app.UseSession();//to can use session


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
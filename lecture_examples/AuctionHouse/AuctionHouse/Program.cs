namespace AuctionHouse;
using AuctionHouse.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AuctionHouse.DAL.Abstract;
using AuctionHouse.DAL.Concrete;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<AuctionHouseDbContext>(
            options => options
                        .UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
                        .UseSqlServer(builder.Configuration.GetConnectionString("AuctionHouseConnection"))
                        
        );
        builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();  // Register our types with the dependency injection container
        //builder.Services.AddScoped<IBidRepository, BidRepository>();  // Register our types with the dependency injection container
        var app = builder.Build();

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
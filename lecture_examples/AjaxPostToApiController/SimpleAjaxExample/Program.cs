using SimpleAjaxExample.DAL;
using SimpleAjaxExample.Models;
using Microsoft.EntityFrameworkCore;

namespace SimpleAjaxExample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        var connectionString = builder.Configuration.GetConnectionString("SomeConnection");
        builder.Services.AddDbContext<SimpleAjaxExampleDbContext>(options => options
                                //.UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
                                .UseSqlServer(connectionString));
        builder.Services.AddScoped<DbContext, SimpleAjaxExampleDbContext>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        // enable Swagger features (needs package Swashbuckle.AspNetCore)
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
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
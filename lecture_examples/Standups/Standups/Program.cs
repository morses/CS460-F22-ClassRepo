using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Standups.Controllers.ActionFilters;
using Standups.DAL.Abstract;
using Standups.DAL.Concrete;
using Standups.Data;
using Standups.Services;
using Standups.Utilities;

namespace Standups
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("AuthenticationConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // Customize some settings that Identity uses
            builder.Services.Configure<IdentityOptions>(opts => {
                // lines without comments are the default settings that we get by "AddDefault" above
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireNonAlphanumeric = false;   // default is true
                opts.Password.RequireUppercase = true;
                opts.Password.RequiredLength = 8;           // default is 6
                opts.Password.RequiredUniqueChars = 5;      // default is 1
                opts.SignIn.RequireConfirmedEmail = false;
                opts.SignIn.RequireConfirmedPhoneNumber = false;
                //opts.User.AllowedUserNameCharacters =
            //"abcdefghijklmnopqrstuvwxyzBCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";   // removed letter 'A'
                opts.User.RequireUniqueEmail = true;        // default is false
            });
            builder.Services.AddControllersWithViews();

            // Change the overriding authorization strategy from 
            //     "allow unless explicitly authorized" (the default)
            // to
            //     "require authorization unless explicitly allowed for anyone"
            // BEWARE!! This will apply to ALL pages, including your Login and Register pages.
            // If you scaffold the Identity pages, remember to put [AllowAnonymous] in front of the code-behind class
            // otherwise you'll get an endless string of redirects: When you go to login with authorize on, it redirects 
            // to the login page, which then redirects to login, etc. until it reaches a limit that the browser presumably sets.
            builder.Services.AddAuthorization(opts => {
                opts.FallbackPolicy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
            });

            var connectionStringApp = builder.Configuration.GetConnectionString("ApplicationConnection");
            builder.Services.AddDbContext<StandupsDbContext>(options => options
                                        .UseLazyLoadingProxies()
                                        .UseSqlServer(connectionStringApp));

            builder.Services.AddScoped<DbContext, StandupsDbContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            // Add our custom Action Filter to set up the user service
            builder.Services.AddScoped<UserServiceFilter>();

            // enable Swagger features (needs package Swashbuckle.AspNetCore)
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // By using a scope for the services to be requested below, we limit their lifetime to this set of calls.
            // See: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#call-services-from-main
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Get the IConfiguration service that allows us to query user-secrets and 
                    // the configuration on Azure
                    var config = app.Services.GetRequiredService<IConfiguration>();
                    // Set password with the Secret Manager tool, or store in Azure app configuration
                    // dotnet user-secrets set SeedUserPW <pw>

                    var testUserPw = config["SeedUserPW"];
                    var adminPw = config["SeedAdminPW"];

                    SeedUsers.Initialize(services, SeedData.UserSeedData, testUserPw).Wait();
                    SeedUsers.InitializeAdmin(services, "admin@example.com", "admin", adminPw, "The", "Admin").Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseMigrationsEndPoint();      // we don't want to trigger migrations to our DB from the running UI!
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
                name: "Admin",
                pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
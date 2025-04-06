using Company.Rabeea.BLL.Interfaces;
using Company.Rabeea.BLL.Repositories;
using Company.Rabeea.DAL.Data.Contexts;
using Company.Rabeea.DAL.Models;
using Company.Rabeea.PL.Authentication;
using Company.Rabeea.PL.Mapping;
using Company.Rabeea.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Rabeea.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allows DI for DepartmentRepository
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(); // Allows DI for DepartmentRepository

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            }); // Allows DI for DbContext
            builder.Services.AddScoped<IScopedService, ScopedService>(); // per request
            builder.Services.AddScoped<ITransientService, TransientService>(); // per operation
            builder.Services.AddScoped<ISingletonService, SingletonService>(); // per application
            builder.Services.AddScoped<IMailService, MailService>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new DepartmentProfile()));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>()
                            .AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(
                config => 
                config.LoginPath = "/Account/SignIn"
                );

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection(nameof(TwilioSettings)));

            // Dependency Injection: Allow clr to create objects of this class instead of the class itself handles it
            // Services LifeTimes

            //builder.Services.AddScoped(); // create object life time per request - Unreachable object after the request -- best for repositories
            //builder.Services.AddTransient(); // create object life time per operation - every use creates an object
            //builder.Services.AddSingleton(); // create object life time per app

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
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           

            app.Run();
        }
    }
}

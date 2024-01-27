using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using ReportingSystem.Data;
using ReportingSystem.Hubs;
using ReportingSystem.Services;
using System.Configuration;

namespace ReportingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //SQLitePCL.Batteries.Init();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            //builder.Services.AddAuthentication("BasicAuthentication")
            //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            builder.Services.AddScoped<AuthorizeService>();
            builder.Services.AddScoped<CustomersService>();
            builder.Services.AddScoped<CompaniesService>();
            builder.Services.AddScoped<EmployeesService>();
            builder.Services.AddScoped<PositionsService>();
            builder.Services.AddScoped<RollsService>();
            builder.Services.AddScoped<ProjectsCategoriesService>();
            builder.Services.AddScoped<ProjectsService>();
            builder.Services.AddScoped<ReportService>();

            builder.Services.AddSignalR();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            //builder.Services.AddScoped<ProjectsService>();
            //builder.Services.AddScoped<ReportModel>();

            //var config = builder.Configuration.GetSection("TempCustomer");
            //builder.Services.Configure<CustomerModel>(config);

            var app = builder.Build();

            //// Configure SQLite connection
            //app.Configuration["ConnectionStrings:SQLiteConnection"] = "Data Source=mydatabase.db";


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

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "configuration",
                pattern: "configuration",
                defaults: new { controller = "Configuration", action = "Configuration" });

            //app.UseAuthentication();

            //app.MapControllerRoute(
            //    name: "configuration",
            //    pattern: "configuration",
            //    defaults: new { controller = "Configuration", action = "Configuration" })
            //    .RequireAuthorization(); // Додаємо вимогу автентифікації для цього маршруту




            app.MapHub<StatusHub>("/statusHub");

            app.Run();
        }
    }
}
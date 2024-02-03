using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using ReportingSystem.Common;
using ReportingSystem.Data;
using ReportingSystem.Hubs;
using ReportingSystem.Middlewares;
using ReportingSystem.Models.Settings;
using ReportingSystem.Services;
using System.Configuration;
using System.Globalization;

namespace ReportingSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddScoped<AuthorizeService>();
            builder.Services.AddScoped<CustomersService>();
            builder.Services.AddScoped<CompaniesService>();
            builder.Services.AddScoped<EmployeesService>();
            builder.Services.AddScoped<PositionsService>();
            builder.Services.AddScoped<RollsService>();
            builder.Services.AddScoped<ProjectsCategoriesService>();
            builder.Services.AddScoped<ProjectsService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<LocalizationCookiesAttribute>();
            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
            builder.Services.AddControllersWithViews()
                .AddViewLocalization()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<LocalizationCookiesAttribute>();
                    options.Filters.Add<PathCookiesAttribute>();
                });

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

            var app = builder.Build();

            CultureInfo[] supportedCultures = new CultureInfo[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("uk-UA"),
            };

            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("uk-UA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseMiddleware<CultureMiddleware>();

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

            app.MapHub<StatusHub>("/statusHub");

            app.Run();
        }
    }
}
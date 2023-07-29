using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.Repository;
using NLayer.Services.Mapping;
using NLayer.Services.Validations;
using NLayer.WEB.Modules;
using NLayer.WEB.Services;
using System.Reflection;

namespace NLayer.WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>

                option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name));
            });

            builder.Services.AddHttpClient<ProductApiServices>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });
            builder.Services.AddHttpClient<CategoryApiServices>(opt =>
            {
                opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
            });

            builder.Services.AddScoped(typeof(NotFoundFilter<>));

            builder.Services.AddAutoMapper(typeof(MapProfile));
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerbuilder => containerbuilder.RegisterModule(new RepoServiceModule()));

            var app = builder.Build();

            app.UseExceptionHandler("/Home/Error");
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
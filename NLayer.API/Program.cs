using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Repository;
using NLayer.Services.Mapping;
using NLayer.Services.Validations;
using System.Reflection;

namespace NLayer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new ValidateFilterAttribute());
            }).
            AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>()); // todo: Bu methodu incele

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();


            builder.Services.AddScoped(typeof(NotFoundFilter<>));


            builder.Services.AddDbContext<AppDbContext>(x =>
            {
                x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>

                option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name));
            });

            #region Old DepencyInjections
            //builder.Services.AddScoped<IUnitOfWorks, UnitOfWork>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
            //builder.Services.AddScoped<IProductRepository, ProductRepository>();
            //builder.Services.AddScoped<IProductService, ProductService>();
            //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            //builder.Services.AddScoped<ICategoryService, CategoryService>(); 
            #endregion


            builder.Services.AddAutoMapper(typeof(MapProfile)); /// todo: Burasý Profile da yapýlabilir miydi ?? 
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerbuilder => containerbuilder.RegisterModule(new RepoServiceModule()));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();  //todo: app'le baþlayýp use ile devam edenler Middleware
            }

            app.UseHttpsRedirection();

            app.UseCustomException();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
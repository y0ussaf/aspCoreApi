using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Tp2.Extensions
{
    public static class ServiceExtensions
    {


        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                 new OpenApiInfo
                 {
                     Title = "Tp2 Api",
                     Version = "v1",
                     Description = "Tp2  ",
                     Contact = new OpenApiContact()
                     {
                         Name = "youssef",
                         Email = "youssefdouirek1@gmail.com"
                     }

                 });
                var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        

        public static void ConfigureDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            var conn = configuration["ConnectionString:devDb"];

            services.AddDbContext<Tp2DbContext>(options => {
           
                options.UseSqlServer(conn,b => b.MigrationsAssembly("Tp2"));
            });
        }
        public static void ConfigureCors (this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                 {
                     builder.AllowAnyOrigin()
                             .AllowAnyHeader()
                             .AllowAnyMethod();
                 });
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {
                
            });
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }


    }
}

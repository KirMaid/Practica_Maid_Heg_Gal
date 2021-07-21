using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CompareServer.Models;


namespace CompareServer
{
    public class Startup
    {
        /// Путь к директории с файлами на сервере
        public static string PathFiles { get => mPathFile; }
        private static string mPathFile;

        /// Путь к директории с шинглами на сервере
        public static string PathShingles { get => mPathShingles; }
        private static string mPathShingles;

        /// Строка подключениия к бд
        public static string ConnectionString { get => mConnectionString; }
        private static string mConnectionString;



        public Startup(IHostingEnvironment env)
        {
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            // Добавление строки подключения к БД
            //DbProvider.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            mConnectionString = Configuration.GetConnectionString("DefaultConnection");
            // Директория для хранения файлов
            mPathFile = Configuration.GetValue<string>("DefaultPathFiles");
            // Директория для хранения шинглов
            mPathShingles = Configuration.GetValue<string>("DefaultPathShingles");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}

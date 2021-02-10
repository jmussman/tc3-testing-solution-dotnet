using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TC3.Models;
using TC3.Services;
using Microsoft.EntityFrameworkCore;

namespace TC3 {
    public class Startup {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            // Modified the database context to use Sqlite3
            // services.AddDbContext<TC3Context>(options => options.UseSqlServer(Configuration.GetConnectionString(name: "DefaultConnection")).EnableSensitiveDataLogging());
            services.AddDbContext<TC3Context>(options => options.UseSqlite(Configuration.GetConnectionString(name: "DefaultConnection")).EnableSensitiveDataLogging());

            // Added the service classes as singletons
            services.AddScoped<ISalesOrdersService, SalesOrdersService>();
            services.AddScoped<ISalesOrderManager, SalesOrderManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

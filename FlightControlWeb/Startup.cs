using System.Collections.Concurrent;
using System.Collections.Generic;
using FlightControlWeb.DataBaseClasses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlightControlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddControllers();
            services.AddRouting();
            //databases
            services.AddSingleton(typeof(IDictionary<string,FlightPlan>),
                typeof(ConcurrentDictionary<string,FlightPlan>));
            services.AddSingleton(typeof(IDictionary<string, Server>),
                typeof(ConcurrentDictionary<string, Server>));
            services.AddSingleton(typeof(IDictionary<string, string>),
                typeof(ConcurrentDictionary<string, string>));
            //client
            services.AddHttpClient("api", client =>
                client.DefaultRequestHeaders.Add("Accept", "application/json"));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

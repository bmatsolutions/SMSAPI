using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SMSAPI.Middlewares;
using SMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnv)
        {
            Configuration = configuration;
            HostEnvironment = hostEnv;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddControllers().AddNewtonsoftJson();
            var config = Util.Util.GetAppConfig(Configuration, HostEnvironment);
            services.Configure<AppConfig>(options =>
            {
                options.ConnectionString = config.ConnectionString;
                options.DatabaseType = config.DatabaseType;
                options.WorkingDir = config.WorkingDir;
                options.LogFile = config.LogFile;
                options.CallLogFile = config.CallLogFile;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/v2/sms"), appBuilder =>
            {
                appBuilder.UseApiAppAuth();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
            });
        }
    }
}

using WebHash.IServices;
using WebHash.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using WebHash.DataModels;
using WebHash.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;

namespace WebHash
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = Env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region dbConfigure
            services.AddDbContext<Context>(
                options => options.UseSqlServer(Configuration.GetConnectionString("WebHashConnection"))
                );

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 209715200; //200 Mb max file size
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = 209715200;
                options.MultipartBodyLengthLimit = 209715200;
            });
            #endregion

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            #region Dependency Injection
            services.AddHttpContextAccessor();
            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IStartProgramService, StartProgramService>();
            services.AddSingleton<ICsvService, CsvService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddSingleton<IAnalyseService, AnalyseService>();

            services.AddHttpClient();

            #endregion


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

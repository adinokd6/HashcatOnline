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
            #endregion

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            #region Dependency Injection
            services.AddHttpContextAccessor();
            services.AddSingleton<IHashService, HashService>();
            services.AddSingleton<IStartProgramService, StartProgramService>();
            services.AddSingleton<ICsvService, CsvService>();
            services.AddSingleton<IFileService, FileService>();

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

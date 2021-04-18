using ClientsExercise.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsExercise
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddCors(o => o.AddPolicy("AppPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddTransient<IDataProvider, AdoDataProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //app.Map("/apps/app1", builder => {
            //    builder.Use(spa =>
            //    {
            //        if (env.IsDevelopment())
            //        {
            //            spa.UseProxyToSpaDevelopmentServer($"http://localhost:4201/");
            //        }
            //        else
            //        {
            //            var staticPath = Path.Combine(
            //                Directory.GetCurrentDirectory(), $"wwwroot/Apps/dist/app1");
            //            var fileOptions = new StaticFileOptions
            //            { FileProvider = new PhysicalFileProvider(staticPath) };
            //            builder.UseSpaStaticFiles(options: fileOptions);

            //            spa.Options.DefaultPageStaticFileOptions = fileOptions;
            //        }
            //    });
            //});


            app.UseHttpsRedirection();

            //var extensionProvider = new FileExtensionContentTypeProvider();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "wwwroot")),
            //    RequestPath = "/ddd",
            //    ContentTypeProvider = extensionProvider,
            //    ServeUnknownFileTypes = true
            //});


            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    if (context.Response.StatusCode == 404 && !System.IO.Path.HasExtension(context.Request.Path.Value))
            //    {
            //        context.Request.Path = "/index.html";
            //        context.Response.StatusCode = 200;
            //        await next();
            //    }
            //});


            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("/index.html");
            app.UseDefaultFiles(options);

            app.UseStaticFiles();

            app.UseFileServer(enableDirectoryBrowsing: false);

            app.UseRouting();


            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

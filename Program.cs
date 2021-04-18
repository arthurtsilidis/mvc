using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsExercise
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    //.UseKestrel()
                    //.UseContentRoot(Directory.GetCurrentDirectory())
                    //.UseSetting("detailedErrors", "true")
                    //.UseIISIntegration()
                    .UseStartup<Startup>();
                    //.CaptureStartupErrors(true);
                });
    }
}

using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ServerSentEventSample
{
    public class Program
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDirectoryBrowser();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvcCore();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment host)
        {
            app.UseFileServer(new FileServerOptions
            {
                EnableDirectoryBrowsing = true
            });

            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
                .UseStartup<Program>()
                .Build();

            host.Run();
        }
    }
}



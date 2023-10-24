using Estoque.Api.Configuration;
using Estoque.Api.Interfaces;

namespace Estoque.Api
{
    public class Startup : IStartupApplication
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(hostEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
             .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddDependencyConfig();
            services.AddSwaggerConfig();
            services.AddIdentityConfig(Configuration);
            services.AddApiConfig(Configuration);
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfig();
            app.UseApiConfig(env);
        }
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder webApplicationBuilder) where TStartup : IStartupApplication
        {
            var startup = Activator.CreateInstance(typeof(TStartup), webApplicationBuilder.Environment) as IStartupApplication;
            if (startup == null) throw new("Classe startup.cs inválida");

            startup.ConfigureServices(webApplicationBuilder.Services);
            var app = webApplicationBuilder.Build();
            startup.Configure(app, app.Environment);
            app.Run();

            return webApplicationBuilder;
        }
    }
}

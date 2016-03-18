using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microbrewit.Api.Mapper;
using Microbrewit.Api.Settings;
using Microbrewit.Api.Repository.Component;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Service.Component;
using Microbrewit.Repository.Repository;

namespace microbrewit_api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ElasticSearchSettings>(Configuration.GetSection("ElasticSearch"));
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            
            services.AddTransient<IOriginRespository,OriginDapperRepository>();
            services.AddTransient<IOriginService,OriginService>();
            services.AddTransient<IHopRepository,HopDapperRepository>();
            services.AddTransient<IHopService,HopService>();
            services.AddTransient<IYeastRepository, YeastDapperRepository>();
            services.AddTransient<IYeastService, YeastService>();
            services.AddTransient<IFermentableRepository, FermentableDapperRepository>();
            services.AddTransient<IFermentableService, FermentableService>();
            services.AddTransient<IOtherRepository, OtherDapperRepository>();
            services.AddTransient<IOtherService, OtherService>();
            services.AddTransient<ISupplierRepository, SupplierDapperRepository>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IBeerStyleRepository, BeerStyleDapperRepository>();
            services.AddTransient<IBeerStyleService, BeerStyleService>();
            services.AddTransient<IBeerRepository, BeerDapperRepository>();
            services.AddTransient<IBeerService, BeerService>();
            services.AddTransient<IBreweryRepository, BreweryDapperRepository>();
            services.AddTransient<IBreweryService, BreweryService>();

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Verbose);

            app.UseIISPlatformHandler();

            app.UseStaticFiles();
           
            app.UseMvc();
            AutoMapperConfiguration.Configure();
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}

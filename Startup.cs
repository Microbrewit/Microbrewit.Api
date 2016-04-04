using Microbrewit.Api.ElasticSearch.Component;
using Microbrewit.Api.ElasticSearch.Interface;
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
using Microbrewit.Api.Calculations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNet.Mvc.Formatters;

namespace Microbrewit.Api
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
            //Repository dependency injection
            services.AddTransient<IOriginRespository,OriginDapperRepository>();
            services.AddTransient<IHopRepository,HopDapperRepository>();
            services.AddTransient<IYeastRepository, YeastDapperRepository>();
            services.AddTransient<IFermentableRepository, FermentableDapperRepository>();
            services.AddTransient<IOtherRepository, OtherDapperRepository>();
            services.AddTransient<ISupplierRepository, SupplierDapperRepository>();
            services.AddTransient<IBeerStyleRepository, BeerStyleDapperRepository>();
            services.AddTransient<IBeerRepository, BeerDapperRepository>();
            services.AddTransient<IBreweryRepository, BreweryDapperRepository>();
            services.AddTransient<IOriginService,OriginService>();
            //Service dependency injection
            services.AddTransient<IHopService,HopService>();
            services.AddTransient<IYeastService, YeastService>();
            services.AddTransient<IFermentableService, FermentableService>();
            services.AddTransient<IOtherService, OtherService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<IBeerStyleService, BeerStyleService>();
            services.AddTransient<IBeerService, BeerService>();
            services.AddTransient<IBreweryService, BreweryService>();
            services.AddTransient<IIngredientService, IngredientService>();
            //ElasticSearch dependency injection
            services.AddTransient<IBeerElasticsearch, BeerElasticsearch>();
            services.AddTransient<IBeerStyleElasticsearch, BeerStyleElasticsearch>();
            services.AddTransient<IBreweryElasticsearch, BreweryElasticsearch>();
            services.AddTransient<IFermentableElasticsearch, FermentableElasticsearch>();
            services.AddTransient<IGlassElasticsearch, GlassElasticsearch>();
            services.AddTransient<IHopElasticsearch, HopElasticsearch>();
            services.AddTransient<IOriginElasticsearch, OriginElasticsearch>();
            services.AddTransient<IOtherElasticsearch, OtherElasticsearch>();
            services.AddTransient<ISearchElasticsearch, SearchElasticsearch>();
            services.AddTransient<IYeastElasticsearch, YeastElasticsearch>();

            services.AddTransient<ICalculation,Calculation>();
            services.AddTransient<IBeerXmlResolver,BeerXmlResolver>();
            // Add framework services.
            services.AddMvc(config =>
                {
                  // Add XML Content Negotiation
                  config.RespectBrowserAcceptHeader = true;
                  config.InputFormatters.Add(new XmlSerializerInputFormatter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Verbose);

            app.UseIISPlatformHandler();
             app.UseCors(policy =>
            {
                policy.WithOrigins("https://microbrew.it","http://microbrew.it", "http://localhost:3000");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseIdentityServerAuthentication(options =>
            {
                options.Authority = "http://auth.microbrew.it";
                options.ScopeName = "microbrewit-api";
                options.ScopeSecret = "secret";

                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
            });
            app.UseStaticFiles();
           
            app.UseMvc();
            AutoMapperConfiguration.Configure();                      
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}

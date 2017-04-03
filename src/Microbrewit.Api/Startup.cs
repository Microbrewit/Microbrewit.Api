using System.IdentityModel.Tokens.Jwt;
using Microbrewit.Api.Calculations;
using Microbrewit.Api.Configuration;
using Microbrewit.Api.Elasticsearch.Component;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Mapper;
using Microbrewit.Api.Repository.Component;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Component;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Settings;
using Microbrewit.Repository.Component;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Compact;

namespace Microbrewit.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(new CompactJsonFormatter())
                .Enrich.FromLogContext() 
                .CreateLogger();
                
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
 
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ElasticSearchSettings>(options => Configuration.GetSection("ElasticSearch").Bind(options));
            services.Configure<DatabaseSettings>(options => Configuration.GetSection("DatabaseSettings").Bind(options));
            services.Configure<ApiSettings>(options => Configuration.GetSection("ApiSettings").Bind(options));
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
            services.AddTransient<IUserRepository,UserDapperRepository>();
            services.AddTransient<IGlassRepository, GlassDapperRepository>();
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
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IOriginService,OriginService>();
            services.AddTransient<IGlassService, GlassService>();
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
            services.AddTransient<ISupplierElasticsearch, SupplierElasticsearch>();
            services.AddTransient<IUserElasticsearch,UserElasticsearch>();
            services.AddTransient<IIngredientElasticsearch, IngredientElasticsearch>();

            services.AddTransient<ICalculation,Calculation>();
            services.AddTransient<IBeerXmlResolver,BeerXmlResolver>();
            // Add framework services.
            services.AddMvc(config =>
                {
                  // Add XML Content Negotiation
                  config.RespectBrowserAcceptHeader = true;
                  //config.InputFormatters.Add(new XmlSerializerInputFormatter());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug(LogLevel.Debug);

            ApiConfiguration.ApiSettings = app.ApplicationServices.GetService<IOptions<ApiSettings>>().Value;
            app.UseCors(policy =>
            {
                policy.WithOrigins("https://microbrew.it","http://microbrew.it", "http://localhost:3000", "http://calc.asphaug.io");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                Authority = "http://auth.microbrew.it/",
                Audience = "http://auth.microbrew.it/resources",
                AutomaticAuthenticate = true,
                RequireHttpsMetadata=false,
                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                }
            });
            
            // options =>
            // {
            //     options.Authority = "http://auth.microbrew.it/"
            //     options.ScopeName = "microbrewit-api";
            //     options.ScopeSecret = "secret";

            //     options.AutomaticAuthenticate = true;
            //     options.AutomaticChallenge = true;
            // }
            
            app.UseStaticFiles();
           
           
            app.UseMvc();
            AutoMapperConfiguration.Configure();                      
        }
    }
}

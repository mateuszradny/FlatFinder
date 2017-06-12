using FlatFinder.Application.Services;
using FlatFinder.Contracts.Services;
using FlatFinder.Persistence;
using FlatFinder.Web.Services;
using FlatFinder.WebAPI.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;

namespace FlatFinder.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenValidationOptions>(Configuration.GetSection(nameof(TokenValidationOptions)));

            services.AddMvc();
            services.AddOptions();

            ICryptographyService cryptographyService = new CryptographyService();
            services.AddScoped(_ => new FlatFinderContext(Configuration.GetConnectionString(nameof(FlatFinderContext)), cryptographyService));
            services.AddScoped<ICryptographyService>(x => cryptographyService);
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            ConfigureNodeModules(app, env);
            app.UseStaticFiles();

            ConfigureJwtBearerAuthentication(app);
            app.UseMvc();
        }

        private static void ConfigureNodeModules(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseFileServer(new FileServerOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                    RequestPath = new PathString("/node_modules"),
                    EnableDirectoryBrowsing = true
                });
            }
        }

        private static void ConfigureJwtBearerAuthentication(IApplicationBuilder app)
        {
            TokenValidationOptions tokenValidationOptions = app.ApplicationServices.GetService<IOptions<TokenValidationOptions>>().Value;

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenValidationOptions.Issuer,
                    ValidAudience = tokenValidationOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationOptions.IssuerSigningKey)),
                    ValidateLifetime = true
                }
            });
        }
    }
}
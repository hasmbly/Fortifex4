using System.Text;
using Fortifex4.Application;
using Fortifex4.Application.Common;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Domain.Constants;
using Fortifex4.Infrastructure;
using Fortifex4.Shared;
using Fortifex4.WebAPI.Common;
using Fortifex4.WebAPI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Fortifex4.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);
            services.AddHttpContextAccessor();
            services.AddApplication();
            services.AddControllers();
            services.AddSingleton<ICurrentWeb, CurrentWeb>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = SchemeProvider.Fortifex;
                })
                .AddCookie(SchemeProvider.Fortifex, options =>
                {
                    options.LoginPath = new PathString("/account/login");
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("Fortifex:TokenSecurityKey").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = Configuration[ConfigurationKey.Authentication.Google.ClientId];
                    options.ClientSecret = Configuration[ConfigurationKey.Authentication.Google.ClientSecret];
                    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                    options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
                    options.SaveTokens = true;
                })
                .AddFacebook(options =>
                {
                    options.AppId = Configuration[ConfigurationKey.Authentication.Facebook.AppId];
                    options.AppSecret = Configuration[ConfigurationKey.Authentication.Facebook.AppSecret];
                });

            var physicalProvider = new PhysicalFileProvider(Configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>().ProjectDocumentsRootFolderPath);
            services.AddSingleton<IFileProvider>(physicalProvider);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
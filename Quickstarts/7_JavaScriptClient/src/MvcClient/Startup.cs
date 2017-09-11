using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

	        services.AddAuthentication(options => {
		        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
	        })
			.AddCookie()
			.AddOpenIdConnect(options =>
	        {
		        options.Authority = "http://localhost:5000";
		        options.RequireHttpsMetadata = false;

		        options.ClientId = "mvc";
		        options.ClientSecret = "secret";

		        options.ResponseType = "code id_token";
				options.Scope.Add("api1");
		        options.Scope.Add("offline_access");

				options.GetClaimsFromUserInfoEndpoint = true;
		        options.SaveTokens = true;

	        });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

	        app.UseAuthentication();
			app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
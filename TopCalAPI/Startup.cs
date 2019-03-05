using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TopCal.Data;
using TopCal.Data.Entities;
using TopCal.Data.Identity;
using TopCal.Data.Repository;
using TopCal.Data.Seed;
using TopCalAPI.Config;

namespace TopCalAPI
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration,
                       ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionStrings = Configuration.GetConnectionString("TopCalDb");
            var efAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionStrings,
                    sql => sql.MigrationsAssembly(efAssembly));
            });


            //Register Dependencies
            services.AddScoped<IRepository, Repository>();
            services.AddTransient<ApplicationDbInitializer>();
            services.AddTransient<ApplicationIdentityInitializer>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            ConfigureUnauthorizedRedirect(services);
            ConfigureAuthenticationAndAuthorization(services);
           

            AddCors(services);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env,
                              ApplicationDbInitializer dbInitializer,
                              ApplicationIdentityInitializer identityInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();

            dbInitializer.Seed().Wait();
            identityInitializer.Seed().Wait();
        }

        private void ConfigureAuthenticationAndAuthorization(IServiceCollection services)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = Configuration["AppSettings:Token:Issuer"],
                    ValidAudience = Configuration["AppSettings:Token:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Token:Key"])),
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("UserManagers", p =>
                    {
                        p.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == "Admin" || c.Type == "Manager"));
                    });

                options.AddPolicy("MealManagers", p =>
                    {
                        p.RequireAssertion(context =>
                            context.User.HasClaim(c => c.Type == "Admin" || c.Type == "Regular"));
                    });
            });
        }

        private void AddCors(IServiceCollection services)
        {
            services.AddCors(cfg =>
                cfg.AddPolicy("LocalClient",
                    pol => pol.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")));
        }

        private void ConfigureUnauthorizedRedirect(IServiceCollection services)
        {
            services.ConfigureApplicationCookie(config =>
            {
                config.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}

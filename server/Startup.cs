using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Server.Database;
using Server.Models;
using Server.Services;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnection = Configuration["TRAINS_DB"] ??
                               "Host=localhost;Database=TrainSystem;Username=admin;Password=admin1";
            services.AddDbContext<TrainSystemContext>(opt =>
            {
                opt.UseNpgsql(dbConnection);
            });

            services.AddTransient<TrainStopService>();
            services.AddTransient<RouteService>();
            services.AddTransient<DiscountService>();
            services.AddTransient<TrainService>();
            services.AddTransient<RideService>();

            services.AddScoped<JwtService>();
            
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<TrainSystemContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearer =>
            {
                bearer.RequireHttpsMetadata = false;
                bearer.SaveToken = false;
                bearer.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Configuration.GetSecretKey()),
                };
            });

            services.Configure<IdentityOptions>(conf =>
            {
                conf.Password.RequireDigit = false;
                conf.Password.RequireLowercase = true;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                
                conf.Password.RequiredLength = 5;
                conf.Password.RequiredUniqueChars = 5;

                conf.SignIn.RequireConfirmedEmail = false;
                conf.User.RequireUniqueEmail = true;
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TrainSystemContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            context.Database.Migrate();
        }
    }
}

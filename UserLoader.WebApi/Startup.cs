using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using Serilog;

using System.Collections.Generic;
using System.Text;

using UserLoader.Composition;
using UserLoader.Mq.RabbitMq;
using UserLoader.Operations;
using UserLoader.WebApi.Authentication;

namespace UserLoader.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Bootstrap.Configure(services, Configuration);
            services.Configure<JwtConfiguration>(Configuration.GetSection("Jwt"));
            services.AddAuthorization();
            services.AddControllers();

            services
              .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidateAudience = false,
                      ValidIssuer = Configuration["Jwt:Issuer"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                  };
              });

            services.AddRabbitMq(
                Configuration["RabbitMq:hostname"],
                Configuration["RabbitMq:senderQueue"],
                Configuration["RabbitMq:receiverQueue"]
            );

            services.AddTransient<IUserReader, UserOperations>();
            services.AddTransient<IUserWriter, MqUserWriter>();
            services.AddTransient<IAuthenticationTokenProvider, AuthenticationTokenProvider>();
            services.AddTransient<IUserService>(services =>
            {
                var users = GetUsers();
                return new UserService(users, services.GetRequiredService<IAuthenticationTokenProvider>());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IEnumerable<UserAuthenticationModel> GetUsers() => Configuration.GetSection("Users").Get<List<UserAuthenticationModel>>();
    }
}

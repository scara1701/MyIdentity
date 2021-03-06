using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyIdentity.API.Authentication.Data;
using MyIdentity.API.Authentication.Models;
using MyIdentity.API.DataAccess;
using MyIdentity.API.Internal.DataAccess;
using MyIdentity.API.Services;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MyIdentity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Gwen - trial
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            ITenantService tenantService = new TenantService(httpContextAccessor, Configuration);
        //Gwen - Identity
        services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            //Gwen - Services
            services.AddTransient<IEmailSender, EmailSender>();

            //Gwen - To resolve connection string based upon site address used by client
            services.AddTransient<ITenantService, TenantService>();

            //Gwen - Service to save and retrieve data from sql
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();

            //Gwen - Service to retrieve/set userdata
            services.AddTransient<IUserData, UserData>();

            //Gwen - Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //Gwen - Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    //ValidateAudience = true,
                    //ValidAudience = Configuration["JWT:ValidAudience"],
                    //ValidAudience = Configuration["JWT:Issuer"],
                    //ValidIssuer = Configuration["JWT:Issuer"],
                    //////ValidAudience = services.BuildServiceProvider().GetService<ITenantService>().GetTokenIssuer(),
                    //////ValidIssuer = services.BuildServiceProvider().GetService<ITenantService>().GetTokenIssuer(),
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(services.BuildServiceProvider().GetService<ITenantService>().GetTokenSecret()))

                    //Don't know what I am doing here... https://www.carlrippon.com/asp-net-core-web-api-multi-tenant-jwts/
                    //Does resolve the secret, need to resolve audience and issuer
                    //AudienceValidator?
                    //IssuerValidator
                    AudienceValidator = (IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                    {
                        //if(audiences.FirstOrDefault()== services.BuildServiceProvider().GetService<ITenantService>().GetTokenIssuer())return true;
                        if (audiences.FirstOrDefault() == tenantService.GetTokenIssuer()) return true;
                        return false;
                    },
                    IssuerValidator = (string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                    {
                        //return services.BuildServiceProvider().GetService<ITenantService>().GetTokenIssuer();
                        return tenantService.GetTokenIssuer();
                    },
                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                    {
                        List<SecurityKey> keys = new List<SecurityKey>();
                        //var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(services.BuildServiceProvider().GetService<ITenantService>().GetTokenSecret()));
                        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tenantService.GetTokenSecret()));
                        keys.Add(signingKey);
                        return keys;
                    }
                };
            }
            );

            //Gwen - Needed to recognize url used to access site
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            //Gwen - Swagger
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyIdentity.API", Version = "v1" });
            //});
            services.AddSwaggerGen(swagger =>
            {
                //Gwen - standard UI for Swagger documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Name of Web API",
                    Description = "Description of WEB API"
                }
                );
                //Gwen - Enable auth on Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below. \r\n\r\nExample: \"Bearer eySDFsdfDFGDSfgERtdfgfhfg\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type =ReferenceType.SecurityScheme,
                                Id="Bearer",
                            }
                        },
                        new string[]{}
                    }
                });
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Name of WEB API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //Gwen - Use authentication - Needs to be before Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

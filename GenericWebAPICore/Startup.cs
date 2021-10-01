using System.Threading.Tasks;
using DynamicAndGenericControllersSample.DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using GenericWebAPICore.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.OData.ModelBuilder;

namespace DynamicAndGenericControllersSample
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
            services.AddControllers().AddNewtonsoftJson();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });
            services.AddSingleton(typeof(GenericRepository<>));
            services.AddSingleton(typeof(EFCoreContext));
            services.AddScoped<IUserService, UserService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddDbContext<EFCoreContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("conString")));
            services.
                AddMvc(o => o.Conventions.Add(new GenericControllerRouteConvention())).
                AddNewtonsoftJson(o => { o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; })
                .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new RemoteControllerFeatureProvider()))
                .AddMvcOptions(x => x.EnableEndpointRouting = false)
                .AddOData(o=> {
                    //o.AddModel("odata", GetEdmModel());
                    o.Select();
                    o.Filter();
                    o.Expand();
                    o.Filter();
                    o.OrderBy();
                    o.Count();
                });
            

            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "GenericWebAPICore", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter Jwt bearer token **only**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme,new string[]{ } }
                });

                var appKeySecurityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "appKey",
                    Reference = new OpenApiReference { Id = "appKey", Type = ReferenceType.SecurityScheme }
                };

                x.AddSecurityDefinition(appKeySecurityScheme.Reference.Id, appKeySecurityScheme);
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { appKeySecurityScheme,new string[]{ } }
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.UseEndpoints(x =>x.MapControllers());

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.EnableDependencyInjection();
            //    endpoints.Select().Count().Filter().OrderBy().MaxTop(100).SkipToken().Expand();
            //    endpoints.MapControllers();

            //});

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
            });
        }
    }
}

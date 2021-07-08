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
            services.AddSingleton(typeof(GenericRepository<>));
            services.AddSingleton(typeof(EFCoreContext));
            //services.AddDbContext<EFCoreContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("conString")));
            services.
                AddMvc(o => o.Conventions.Add(new GenericControllerRouteConvention())).
                AddNewtonsoftJson(o =>{ o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;})
                .ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new RemoteControllerFeatureProvider()))
                .AddMvcOptions(x=> x.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}

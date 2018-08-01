using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Game;
using Game.ActionGetters;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace WebAPI
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
            var serviceProvider = new ServiceCollection()
            .AddMediatR(typeof(Resolver).GetTypeInfo().Assembly)
            .AddScoped<IMediator, Mediator>()
            .AddScoped<IColonistPickGetter, ColonistPickGetter>()
            .AddScoped<IDrawGetter, DrawGetter>()
            .AddScoped<IDiscardGetter, DiscardGetter>()
            .AddScoped<IPowerGetter, PowerGetter>()
            .AddScoped<IBuildGetter, BuildGetter>()
            .AddScoped<IGameEndGetter, GameEndGetter>()
            .BuildServiceProvider();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("colonizers", new Info { Title = "Colonizers game API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/colonizers/swagger.json", "Colonizers game API");
                c.RoutePrefix = string.Empty; // set localhost as the swagger endpoint
            });

            app.UseMvc();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using BattleShipServer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebSocketManager;

namespace BattleShipServer
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
			MatchDBContext dbContext = new MatchDBContext(Configuration.GetConnectionString("DefaultConnection"));
			services.AddMvc();
			services.AddSingleton<MatchDBContext>(dbContext);
			services.AddSingleton<WebSocketHandler>(new WebSocketHandler(new WebSocketConnectionManager(dbContext)));
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
			app.UseWebSockets();
			app.Map("/Fire", _app => _app.UseMiddleware<WebSocketManagerMiddleware>(serviceProvider.GetService<WebSocketHandler>()));
        }
    }
}

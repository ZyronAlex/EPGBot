using EPGBot.Adapters;
using EPGBot.Bots;
using EPGBot.Dialogs;
using EPGManager.Interfaces;
using EPGManager.Models;
using EPGManager.Repositories;
using EPGManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EPGBot
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(
            options => options.UseInMemoryDatabase("EPGBot"),
            ServiceLifetime.Singleton);

            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the storage we'll be using for User and Conversation state. (Memory is great for testing purposes.)
            services.AddSingleton<IStorage, MemoryStorage>();

            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

           
            services.AddSingleton<MainDialog>();
            services.AddSingleton<UserProfileDialog>();
            services.AddSingleton<ListChannelDialog>();
            services.AddSingleton<FindChannelDialog>();
            services.AddSingleton<ScheudleDialog>();
            services.AddSingleton<ScheduleChannelDialog>(); 

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, DialogAndWelcomeBot<MainDialog>>();

            services.AddTransient<IEPGRepository, EPGRepository>();
            services.AddTransient<IEPGService, EPGService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEPGService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            // app.UseHttpsRedirection();

            service.ImporteEPG();
        }
    }
}

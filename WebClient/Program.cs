using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;
using WebClient.Services;

namespace WebClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services
                .AddBlazorise(options =>
               {
                   options.ChangeTextOnKeyPress = true;
               })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons()
                .AddSingleton<DragAndDropService>();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient<IMemberDataService, MemberDataService>(client => client.BaseAddress = new Uri("https://localhost:5001/api/"));
            builder.Services.AddHttpClient<ITaskDataService, ManageTasksDataService>(client => client.BaseAddress = new Uri("https://localhost:5001/api/"));

            var host = builder.Build();

            host.Services
            .UseBootstrapProviders()
            .UseFontAwesomeIcons();

            await host.RunAsync();
        }
    }
}
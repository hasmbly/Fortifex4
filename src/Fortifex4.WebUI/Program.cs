using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Fortifex4.WebUI.Common;
using Fortifex4.WebUI.Common.StateContainer;
using Fortifex4.WebUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fortifex4.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddTransient(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            builder.Services.AddScoped<ActivateMemberState>();
            builder.Services.AddScoped<ToggleCheckboxState>();
            builder.Services.AddScoped<GlobalState>();
            builder.Services.AddScoped<ProjectState>();
            builder.Services.AddScoped<WalletTransactionsState>();

            builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IToolsService, ToolsService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IMembersService, MembersService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IRegionsService, RegionsService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IGendersService, GendersService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<ICountriesService, CountriesService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<ITimeFramesService, TimeFramesService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<ICurrenciesService, CurrenciesService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IWalletsService, WalletsService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IBlockchainsService, BlockchainsService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IInternalTransfersService, InternalTransfersService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IExternalTransfersService, ExternalTransfersService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IOwnersService, OwnersService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<ITradesService, TradesService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IPortfolioService, PortfolioService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IProjectsServices, ProjectsServices>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IProjectsDocumentService, ProjectsDocumentService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IDevService, DevService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });
            builder.Services.AddHttpClient<IChartsService, ChartsService>(x => { x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress); });

            var host = builder.Build();

            await host.RunAsync();
        }
    }
}
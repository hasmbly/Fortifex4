using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.Bitcoin;
using Fortifex4.Application.Common.Interfaces.Crypto;
using Fortifex4.Application.Common.Interfaces.Dogecoin;
using Fortifex4.Application.Common.Interfaces.Email;
using Fortifex4.Application.Common.Interfaces.Ethereum;
using Fortifex4.Application.Common.Interfaces.Fiat;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Application.Common.Interfaces.Hive;
using Fortifex4.Application.Common.Interfaces.Steem;
using Fortifex4.Infrastructure.Bitcoin.BitcoinChain;
using Fortifex4.Infrastructure.Bitcoin.BlockExplorer;
using Fortifex4.Infrastructure.Bitcoin.FakeChain;
using Fortifex4.Infrastructure.Bitcoin.SoChain;
using Fortifex4.Infrastructure.Bitocin.Fake;
using Fortifex4.Infrastructure.Constants;
using Fortifex4.Infrastructure.Crypto.CoinMarketCap;
using Fortifex4.Infrastructure.Crypto.Fake;
using Fortifex4.Infrastructure.Dogecoin.DogeChain;
using Fortifex4.Infrastructure.Dogecoin.Fake;
using Fortifex4.Infrastructure.Email.Fake;
using Fortifex4.Infrastructure.Email.SendGrid;
using Fortifex4.Infrastructure.Ethereum.Ethplorer;
using Fortifex4.Infrastructure.Ethereum.Fake;
using Fortifex4.Infrastructure.Ethereum.FakeChain;
using Fortifex4.Infrastructure.Fiat.Fake;
using Fortifex4.Infrastructure.Fiat.Fixer;
using Fortifex4.Infrastructure.File.Default;
using Fortifex4.Infrastructure.Hive.Fake;
using Fortifex4.Infrastructure.Hive.OpenHive;
using Fortifex4.Infrastructure.Logging;
using Fortifex4.Infrastructure.Persistence;
using Fortifex4.Infrastructure.Services;
using Fortifex4.Infrastructure.Steem.Fake;
using Fortifex4.Infrastructure.Steem.Steemit;
using Fortifex4.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fortifex4.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var fortifexOptions = configuration.GetSection(FortifexOptions.RootSection).Get<FortifexOptions>();

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IDateTimeOffsetService, DateTimeOffsetService>();
            services.AddTransient<IFileService, DefaultFileService>();

            services.AddDbContext<Fortifex4DBContext>(options =>
                        options.UseSqlServer(configuration.GetConnectionString("FortifexDatabase")));

            services.AddScoped<IFortifex4DBContext>(provider => provider.GetService<Fortifex4DBContext>());

            services.AddSerilog(configuration);

            AddEmailService(services, fortifexOptions);
            AddFiatService(services, fortifexOptions);
            AddCryptoService(services, fortifexOptions);
            AddEthereumService(services, fortifexOptions);
            AddBitcoinService(services, fortifexOptions);
            AddDogecoinService(services, fortifexOptions);
            AddSteemService(services, fortifexOptions);
            AddHiveService(services, fortifexOptions);

            return services;
        }

        private static void AddEmailService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.EmailServiceProvider)
            {
                case EmailServiceProviders.SendGrid.Name:
                    services.AddTransient<IEmailService, SendGridEmailService>();
                    break;
                default:
                    services.AddTransient<IEmailService, FakeEmailService>();
                    break;
            }
        }

        private static void AddFiatService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.FiatServiceProvider)
            {
                case FiatServiceProviders.Fixer.Name:
                    services.AddTransient<IFiatService, FixerFiatService>();
                    break;
                default:
                    services.AddTransient<IFiatService, FakeFiatService>();
                    break;
            }
        }

        private static void AddCryptoService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.CryptoServiceProvider)
            {
                case CryptoServiceProviders.CoinMarketCap.Name:
                    services.AddTransient<ICryptoService, CoinMarketCapCryptoService>();
                    break;
                default:
                    services.AddTransient<ICryptoService, FakeCryptoService>();
                    break;
            }
        }

        private static void AddEthereumService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.EthereumServiceProvider)
            {
                case EthereumServiceProviders.Ethplorer.Name:
                    services.AddTransient<IEthereumService, EthplorerEthereumService>();
                    break;
                case EthereumServiceProviders.FakeChain.Name:
                    services.AddTransient<IEthereumService, FakeChainEthereumService>();
                    break;
                default:
                    services.AddTransient<IEthereumService, FakeEthereumService>();
                    break;
            }
        }

        private static void AddBitcoinService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.BitcoinServiceProvider)
            {
                case BitcoinServiceProviders.BlockExplorer.Name:
                    services.AddTransient<IBitcoinService, BlockExplorerBitcoinService>();
                    break;
                case BitcoinServiceProviders.BitcoinChain.Name:
                    services.AddTransient<IBitcoinService, BitcoinChainBitcoinService>();
                    break;
                case BitcoinServiceProviders.SoChain.Name:
                    services.AddTransient<IBitcoinService, SoChainBitcoinService>();
                    break;
                case BitcoinServiceProviders.FakeChain.Name:
                    services.AddTransient<IBitcoinService, FakeChainBitcoinService>();
                    break;
                default:
                    services.AddTransient<IBitcoinService, FakeBitcoinService>();
                    break;
            }
        }

        private static void AddDogecoinService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.DogecoinServiceProvider)
            {
                case DogecoinServiceProviders.DogeChain.Name:
                    services.AddTransient<IDogecoinService, DogeChainDogecoinService>();
                    break;
                default:
                    services.AddTransient<IDogecoinService, FakeDogecoinService>();
                    break;
            }
        }

        private static void AddSteemService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.SteemServiceProvider)
            {
                case SteemServiceProviders.Steemit.Name:
                    services.AddTransient<ISteemService, SteemitSteemService>();
                    break;
                default:
                    services.AddTransient<ISteemService, FakeSteemService>();
                    break;
            }
        }

        private static void AddHiveService(IServiceCollection services, FortifexOptions options)
        {
            switch (options.HiveServiceProvider)
            {
                case HiveServiceProviders.OpenHive.Name:
                    services.AddTransient<IHiveService, OpenHiveHiveService>();
                    break;
                default:
                    services.AddTransient<IHiveService, FakeHiveService>();
                    break;
            }
        }
    }
}
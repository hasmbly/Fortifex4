using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fortifex4.Application.Common.Interfaces;
using Fortifex4.Application.Common.Interfaces.File;
using Fortifex4.Domain.Entities;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.System.Commands.SeedMasterData;

namespace Fortifex4.Application.System.Commands.SeedMasterData
{
    public class MasterDataSeeder
    {
        private readonly IDictionary<int, Country> Countries = new Dictionary<int, Country>();
        private readonly IDictionary<int, Region> Regions = new Dictionary<int, Region>();
        private readonly IDictionary<int, Gender> Genders = new Dictionary<int, Gender>();
        private readonly IDictionary<int, TimeFrame> TimeFrames = new Dictionary<int, TimeFrame>();
        private readonly IDictionary<int, Provider> Providers = new Dictionary<int, Provider>();

        private readonly IFortifex4DBContext _context;
        private readonly IFileService _fileService;

        public MasterDataSeeder(IFortifex4DBContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<SeedMasterDataResponse> SeedAllAsync(CancellationToken cancellationToken)
        {
            var result = new SeedMasterDataResponse();

            #region Master Data Level 0 (Strong Entities)
            result.Entities.Add(nameof(Country), await SeedCountriesAsync(cancellationToken));
            result.Entities.Add(nameof(Region), await SeedRegionsAsync(cancellationToken));
            result.Entities.Add(nameof(Gender), await SeedGendersAsync(cancellationToken));
            result.Entities.Add(nameof(TimeFrame), await SeedTimeFramesAsync(cancellationToken));
            result.Entities.Add(nameof(Provider), await SeedProvidersAsync(cancellationToken));
            #endregion

            return result;
        }

        private async Task<int> SeedCountriesAsync(CancellationToken cancellationToken)
        {
            int itemsAdded = 0;
            var countries = _fileService.ReadCSVFile<CountryCSV>("Data/Countries.csv");

            for (int i = 0; i < countries.Count; i++)
            {
                Countries.Add((i + 1), new Country { CountryCode = countries[i].CountryCode, Name = countries[i].Name });
            }

            if (!_context.Countries.Any())
            {
                foreach (var item in Countries.Values)
                {
                    _context.Countries.Add(item);
                    itemsAdded++;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return itemsAdded;
        }

        private async Task<int> SeedRegionsAsync(CancellationToken cancellationToken)
        {
            int itemsAdded = 0;
            var regions = _fileService.ReadCSVFile<RegionCSV>("Data/Regions.csv");

            for (int i = 0; i < regions.Count; i++)
            {
                Regions.Add((i + 1), new Region { CountryCode = regions[i].CountryCode, Name = regions[i].Name });
            }

            if (!_context.Regions.Any())
            {
                foreach (var item in Regions.Values)
                {
                    _context.Regions.Add(item);
                    itemsAdded++;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return itemsAdded;
        }

        private async Task<int> SeedGendersAsync(CancellationToken cancellationToken)
        {
            int itemsAdded = 0;

            Genders.Add(1, new Gender { GenderID = GenderID.Undefined, Name = GenderName.Undefined });
            Genders.Add(2, new Gender { GenderID = GenderID.Male, Name = GenderName.Male });
            Genders.Add(3, new Gender { GenderID = GenderID.Female, Name = GenderName.Female });

            if (!_context.Genders.Any())
            {
                foreach (var item in Genders.Values)
                {
                    _context.Genders.Add(item);
                    itemsAdded++;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return itemsAdded;
        }

        private async Task<int> SeedTimeFramesAsync(CancellationToken cancellationToken)
        {
            int itemsAdded = 0;

            TimeFrames.Add(1, new TimeFrame { TimeFrameID = TimeFrameID.OneHour, Name = TimeFrameName.OneHour });
            TimeFrames.Add(2, new TimeFrame { TimeFrameID = TimeFrameID.OneDay, Name = TimeFrameName.OneDay });
            TimeFrames.Add(3, new TimeFrame { TimeFrameID = TimeFrameID.OneWeek, Name = TimeFrameName.OneWeek });
            TimeFrames.Add(4, new TimeFrame { TimeFrameID = TimeFrameID.LifeTime, Name = TimeFrameName.LifeTime });

            if (!_context.TimeFrames.Any())
            {
                foreach (var item in TimeFrames.Values)
                {
                    _context.TimeFrames.Add(item);
                    itemsAdded++;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return itemsAdded;
        }

        private async Task<int> SeedProvidersAsync(CancellationToken cancellationToken)
        {
            int itemsAdded = 0;

            Providers.Add(0, new Provider
            {
                ProviderID = ProviderID.Personal,
                Name = ProviderName.Personal,
                ProviderType = ProviderType.Personal,
                SiteURL = "N/A"
            });

            var exchanges = _fileService.ReadCSVFile<ExchangeCSV>("Data/Exchanges.csv");

            for (int i = 0; i < exchanges.Count; i++)
            {
                Providers.Add((i + 1), new Provider
                {
                    ProviderID = exchanges[i].ProviderID,
                    Name = exchanges[i].Name,
                    ProviderType = ProviderType.Exchange,
                    SiteURL = exchanges[i].SiteURL
                });
            }

            if (!_context.Providers.Any())
            {
                foreach (var item in Providers.Values)
                {
                    _context.Providers.Add(item);
                    itemsAdded++;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            return itemsAdded;
        }
    }
}

using System;
using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Members.Queries.GetMember
{
    public class GetMemberResult : IMapFrom<Member>
    {
        public string MemberUsername { get; set; }
        public string ExternalID { get; set; }
        public string ClaimType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PictureUrl { get; set; }
        public int GenderID { get; set; }
        public int RegionID { get; set; }
        public int PreferredFiatCurrencyID { get; set; }
        public int PreferredCoinCurrencyID { get; set; }
        public int PreferredTimeFrameID { get; set; }

        public string GenderName { get; set; }
        public string RegionName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PreferredFiatCurrencyName { get; set; }
        public string PreferredFiatCurrencySymbol { get; set; }
        public string PreferredCoinCurrencyName { get; set; }
        public string PreferredCoinCurrencySymbol { get; set; }
        public string PreferredTimeFrameName { get; set; }

        public string BirthDateDisplayText { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Member, GetMemberResult>()
                .ForMember(dto => dto.GenderName, opt => opt.MapFrom(p => p.Gender.Name))
                .ForMember(dto => dto.RegionName, opt => opt.MapFrom(p => p.Region.Name))
                .ForMember(dto => dto.CountryCode, opt => opt.MapFrom(p => p.Region.CountryCode))
                .ForMember(dto => dto.CountryName, opt => opt.MapFrom(p => p.Region.Country.Name))
                .ForMember(dto => dto.PreferredFiatCurrencyName, opt => opt.MapFrom(p => p.PreferredFiatCurrency.Name))
                .ForMember(dto => dto.PreferredFiatCurrencySymbol, opt => opt.MapFrom(p => p.PreferredFiatCurrency.Symbol))
                .ForMember(dto => dto.PreferredCoinCurrencyName, opt => opt.MapFrom(p => p.PreferredCoinCurrency.Name))
                .ForMember(dto => dto.PreferredCoinCurrencySymbol, opt => opt.MapFrom(p => p.PreferredCoinCurrency.Symbol))
                .ForMember(dto => dto.PreferredTimeFrameName, opt => opt.MapFrom(p => p.PreferredTimeFrame.Name))
                .ForMember(dto => dto.BirthDateDisplayText, opt => opt.MapFrom(p => p.BirthDate.ToString("d MMMM yyyy")));
        }
    }
}
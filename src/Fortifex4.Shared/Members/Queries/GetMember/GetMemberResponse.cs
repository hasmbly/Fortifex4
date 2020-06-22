using System;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Members.Queries.GetMember
{
    public class GetMemberResponse : GeneralResponse
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
        
        public string BirthDateDisplayText
        {
            get
            {
                return this.BirthDate.ToString("d MMMM yyyy");
            }
        }
    }
}
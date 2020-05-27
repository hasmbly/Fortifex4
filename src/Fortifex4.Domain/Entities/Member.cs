using Fortifex4.Domain.Common;
using Fortifex4.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Fortifex4.Domain.Entities
{
    public class Member : AuditableEntity
    {
        public string MemberUsername { get; set; }
        public string ExternalID { get; set; }
        public string AuthenticationScheme { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PictureURL { get; set; }
        public int GenderID { get; set; }
        public int RegionID { get; set; }
        public int PreferredFiatCurrencyID { get; set; }
        public int PreferredCoinCurrencyID { get; set; }
        public int PreferredTimeFrameID { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Guid ActivationCode { get; set; }
        public ActivationStatus ActivationStatus { get; set; }

        public Gender Gender { get; set; }
        public Region Region { get; set; }
        public Currency PreferredFiatCurrency { get; set; }
        public Currency PreferredCoinCurrency { get; set; }
        public TimeFrame PreferredTimeFrame { get; set; }

        public IList<Project> Projects { get; private set; }
        public IList<Owner> Owners { get; private set; }

        public Member()
        {
            this.Owners = new List<Owner>();
            this.Projects = new List<Project>();
        }
    }

    public static class MemberBirthDate
    {
        public static DateTime Default = DateTime.Now;
    }
}
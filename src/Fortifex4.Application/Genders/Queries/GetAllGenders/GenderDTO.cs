using AutoMapper;
using Fortifex4.Application.Common.Mappings;
using Fortifex4.Domain.Entities;

namespace Fortifex4.Application.Genders.Queries.GetAllGenders
{
    public class GenderDTO : IMapFrom<Gender>
    {
        public int GenderID { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Gender, GenderDTO>();
        }
    }
}
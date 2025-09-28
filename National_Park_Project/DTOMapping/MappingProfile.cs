using AutoMapper;
using National_Park_Project.Model;
using National_Park_Project.Model.DTOs;

namespace National_Park_Project.DTOMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
        CreateMap<TrailDTO,Trail>().ReverseMap();
        CreateMap<NationalParkDTOs,NationalPark>().ReverseMap();
        }

    }
}

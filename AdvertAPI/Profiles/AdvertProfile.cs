using AdvertAPI.Models;
using AutoMapper;

namespace AdvertApi.Profiles
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<AdvertDbModel, AdvertModel>().ReverseMap();
        }
    }
}
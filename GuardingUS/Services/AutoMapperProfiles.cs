using AutoMapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;

namespace GuardingUS.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Users, EditUserVM>();
        }
    }
}

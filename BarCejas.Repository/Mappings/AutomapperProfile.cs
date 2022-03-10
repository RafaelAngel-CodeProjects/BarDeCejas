using AutoMapper;

using BarCejas.Core.DTOs;
using BarCejas.Core.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}

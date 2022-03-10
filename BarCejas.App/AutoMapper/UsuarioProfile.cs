using AutoMapper;
using BarCejas.App.Models;
using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.AutoMapper
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            _ = CreateMap<UserViewModel, Usuario>().ReverseMap();
            _ = CreateMap<RegistreViewModel, Usuario>().ReverseMap();
        }
    }
}

using AutoMapper;
using BarCejas.App.Areas.Admin.Models;
using BarCejas.Entities;

namespace BarCejas.App.AutoMapper
{
    public class ProfesionalProfile : Profile
    {
        public ProfesionalProfile()
        {
            _ = CreateMap<ProfesionalViewModel, Profesional>().ReverseMap();
        }
    }
}

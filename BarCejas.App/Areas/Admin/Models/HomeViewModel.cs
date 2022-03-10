using BarCejas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.lBanner = new List<Banner>();
            this.lTestimonios = new List<Testimonios>();
        }

        public HomeViewModel(List<Banner> plBanner, List<Testimonios> plTestimonios)
        {
            this.lBanner = plBanner;
            this.lTestimonios = plTestimonios;
        }

        public List<Banner> lBanner { get; set; }

        public List<Testimonios> lTestimonios { get; set; }
    }
}

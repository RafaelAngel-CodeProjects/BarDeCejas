using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface ITestimonial
    {
        Task<bool> InsertTestimonial(Testimonios Testimonial);

        Task<bool> UpdateTestimonial(Testimonios Testimonial);

        Task<Testimonios> GetTestimonialById(long id);

        //PagedList<Novedades> GetTestimonialsAll(QueryFilter filters);

        public IEnumerable<Testimonios> GetAll();

        public Task<bool> ChangeStatus(long Id, bool Estatus);

        public Task<bool> DeleteTestimonial(long pId, int pOrden);

        public Task<bool> Reorder(List<Testimonios> pPreguntas);
        Task<IEnumerable<Testimonios>> GetTestimonialAllActive();
        
    }
}

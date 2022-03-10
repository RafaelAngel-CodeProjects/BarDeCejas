using BarCejas.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class PreguntasFrecuentesViewModel: PreguntasFrecuentes
    {

        public PreguntasFrecuentesViewModel() { 
        
        }

        public PreguntasFrecuentesViewModel(PreguntasFrecuentes pPregunta)
        {
            this.Id = pPregunta.Id;
            this.IndEstatus = pPregunta.IndEstatus;
            this.Orden = pPregunta.Orden;
            this.Pregunta = pPregunta.Pregunta;
            this.Respuesta = pPregunta.Respuesta;
            this.EsEliminado = pPregunta.EsEliminado;
        }
    }
}

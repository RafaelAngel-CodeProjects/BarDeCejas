using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities.Enumerations
{
    public enum EstatusOrden
    {
        No_Selecccionado = 0,
        preOrden = 5,
        pendiente = 10,
        procesando = 20,
        completada = 30,
        Anulada = 40,
    }
}

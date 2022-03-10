using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Entities.Enumerations
{
    /// <summary>
    /// Jerarquía => Administrador > Secretario > Profesional > Cliente
    /// </summary>
    public enum RoleType
    {
        Administrador = 1,
        Profesional = 2,
        Cliente = 3,
        Secretario = 4,
    }
}

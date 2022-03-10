using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Models
{
    public class BlockedHoursViewModel
    {
        public int professionalId { get; set; }
        public TimeSpan InitialHour { get; set; }
        public TimeSpan FinalHour { get; set; }
    }
}

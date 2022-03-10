using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class GoogleProfileViewModel
    {
        public string nameidentifier { get; set; }

        public string name { get; set; }
        public string givenname { get; set; }

        public string surname { get; set; }

        public string emailaddress { get; set; }

        public string picture { get; set; }

    }
}

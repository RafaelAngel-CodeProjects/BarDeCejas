using System;
using System.Collections.Generic;
using System.Text;

namespace BarCejas.Data.Options
{
    public class PasswordOptions
    {
        public int SaltSize { get; set; }
        public int KeySize { get; set; }
        public int Iterations { get; set; }
    }
}

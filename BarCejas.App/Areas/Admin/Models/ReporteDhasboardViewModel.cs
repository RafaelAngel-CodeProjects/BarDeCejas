using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCejas.App.Areas.Admin.Models
{
    public class ReporteDhasboardViewModel
    {
        public ReporteDhasboardViewModel()
        {
            lReporteMes = new List<ReporteMes>();
        }

        public List<ReporteMes> lReporteMes { get; set; }

    }

    public class ReporteMes
    {
        public int CantMes { get; set; }

        public int Mes { get; set; }

        public string strMes
        {
            get
            {
                switch (Mes)
                {
                    case 1:
                        return "Enero";
                        break;
                    case 2:
                        return "Febrero";
                        break;
                    case 3:
                        return "Marzo";
                        break;
                    case 4:
                        return "Abril";
                        break;
                    case 5:
                        return "Mayo";
                        break;
                    case 6:
                        return "Junio";
                        break;
                    case 7:
                        return "Julio";
                        break;
                    case 8:
                        return "Agosto";
                        break;
                    case 9:
                        return "Septiembre";
                        break;
                    case 10:
                        return "Octubre";
                        break;
                    case 11:
                        return "Nombiembre";
                        break;
                    case 12:
                        return "Diciembre";
                        break;
                    default:
                        return "";
                        break;
                };
            }
        }
    }

}

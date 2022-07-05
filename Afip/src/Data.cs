using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibreriaAfip
{
    public class Data
    {
        public string Token { get; set; }
        public string Sign { get; set; }
        public string Cuit { get; set; }
        public string MonId { get; set; }
        public string CbteTipo { get; set; }
        public string CbteNro { get; set; }
        public string PtoVta { get; set; }
        public string CantReg { get; set; }
        public string Concepto { get; set; }
        public string DocTipo { get; set; }
        public string DocNro { get; set; }
        public string CbteDesde { get; set; }
        public string CbteHasta { get; set; }
        public string CbteFch { get; set; }
        public string ImpTotal { get; set; }
        public string ImpTotConc { get; set; }
        public string ImpNeto { get; set; }
        public string ImpOpEx { get; set; }
        public string ImpTrib { get; set; }
        public string ImpIva { get; set; }
        public string FchServDesde { get; set; }
        public string FchServHasta { get; set; }
        public string FchVtoPago { get; set; }
        public string MonCotiz { get; set; }

    }
}

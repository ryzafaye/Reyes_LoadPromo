using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoModels
{
    public class PromoItem
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string DataAllowance { get; set; } = string.Empty;
        public string Freebies { get; set; } = string.Empty;
        public int ValidityDays { get; set; }
    }
}

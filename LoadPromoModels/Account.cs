using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoModels
{
    public class Account
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string PIN { get; set; } = string.Empty;
        public string Network { get; set; } = string.Empty;
        public double WalletBalance { get; set; }
        public double SimLoadBalance { get; set; }
        public string SimLoadExpiry { get; set; } = string.Empty;


        public string ActivePromo { get; set; } = string.Empty;
        public string ActiveData { get; set; } = string.Empty;
        public string ActiveExpiry { get; set; } = string.Empty;
        public string ActiveFreebies { get; set; } = string.Empty;
    }
}

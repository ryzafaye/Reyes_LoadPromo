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
        public string Network { get; set; } = string.Empty;
        public int NetworkID { get; set; }

        public int WalletBalance { get; set; }
        public double TotalPoints { get; set; }
        public int MySimLoad { get; set; }

        public string ActivePromo { get; set; } = "None";
        public string ActiveData { get; set; } = "None";
        public string ActiveExpiry { get; set; } = "None";
        public string ActiveFreebies { get; set; } = "None";
    }
}

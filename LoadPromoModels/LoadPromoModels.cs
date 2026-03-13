using System;

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

    public class Transaction
    {
        public string RefNumber { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public class PromoItem
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string DataAllowance { get; set; } = string.Empty;
        public string Freebies { get; set; } = string.Empty;
        public int ValidityDays { get; set; }

    }
}

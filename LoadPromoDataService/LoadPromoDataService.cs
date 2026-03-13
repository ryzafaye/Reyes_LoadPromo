using System;
using System.Collections.Generic;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public class Data
    {
        public static Account CurrentAccount { get; set; } = new Account();
        public static List<Transaction>TransactionHistory { get; set; } = new List<Transaction>();
    }

    public class AccountHolder 
    {
        public Account GetAccount()
        {
            return Data.CurrentAccount;
        }

        public void UpdateAccount(Account acc)
        {
            Data.CurrentAccount = acc;
        }
    }

    public class TransactionHolder
    {
        public void Add(Transaction t)
        {
            Data.TransactionHistory.Add(t);
        }
        public List<Transaction> GetAll()
        {
            return Data.TransactionHistory;
        }
    }

    public class NetworkHolder
    {
        public List<string> GetAllNetworks()
        {
            return new List<string> {"TM", "GLOBE", "SMART", "TNT", "DITO"};
        }
    }
    public class LoadOptionsHolder
    {
        public List<int> GetRegLoadOptions()
        {
            return new List<int> {50, 100, 200, 300, 500};
        }
    }

    public class PromoHolder
    {
        public List<PromoItem> GetPromosByNetwork(int networkId)
        {
            switch (networkId)
            {
                case 1:
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "All-Net SURF 30", Price = 30, DataAllowance = "150 MB/day + 750 MB", Freebies = "Unli AllNet Texts", ValidityDays = 2 },
                        new PromoItem { Name = "EasySURF50", Price = 50, DataAllowance = "6GB of data", Freebies = "Unli AllNet Calls & Texts", ValidityDays = 3 },
                        new PromoItem { Name = "EasySURF99", Price = 99, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts", ValidityDays = 7 },
                        new PromoItem { Name = "EasySURF140", Price = 140, DataAllowance = "18GB of data", Freebies = "Unli AllNet Calls & Texts", ValidityDays = 7 },
                        new PromoItem { Name = "TM EasyPlan 150", Price = 150, DataAllowance = "30GB of data", Freebies = "Unli AllNet Calls & Texts", ValidityDays = 15 }
                    };
                case 2:
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "GoUNLI30", Price = 30, DataAllowance = "100 MB", Freebies = "Unli AllNet Texts", ValidityDays = 2 },
                        new PromoItem { Name = "Go59 for Students", Price = 59, DataAllowance = "5GB + 1GB GoLEARN", Freebies = "Unli AllNet Texts", ValidityDays = 3 },
                        new PromoItem { Name = "GoPLUS99", Price = 99, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts", ValidityDays = 7 },
                        new PromoItem { Name = "GoPLUS129", Price = 129, DataAllowance = "18GB of data", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 7 },
                        new PromoItem { Name = "GoPLUS250", Price = 250, DataAllowance = "30GB of data", Freebies = "15GB for apps", ValidityDays = 15 }
                    };
                case 3:
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "SURFSAYA 35", Price = 35, DataAllowance = "500MB/day + 900MB", Freebies = "Unli AllNet Texts", ValidityDays = 3 },
                        new PromoItem { Name = "POWER ALL 59", Price = 59, DataAllowance = "5GB + 3GB 5G", Freebies = "Unli AllNet Calls & Texts", ValidityDays = 3 },
                        new PromoItem { Name = "SAYA ALL 99", Price = 99, DataAllowance = "6GB + UNLI Apps", Freebies = "Unli Calls & Texts", ValidityDays = 7 },
                        new PromoItem { Name = "NON-STOP DATA 115", Price = 115, DataAllowance = "Unli 5G + NON-STOP 4G", Freebies = "Unli Calls & Texts", ValidityDays = 3 },
                        new PromoItem { Name = "Giga Data 349", Price = 349, DataAllowance = "16GB Data", Freebies = "Unli Calls & Texts", ValidityDays = 30 }
                    };
                case 4: 
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "SURFSAYA 49", Price = 49, DataAllowance = "1.2GB + 250MB/day", Freebies = "Unli Calls & Texts", ValidityDays = 3 },
                        new PromoItem { Name = "SAYA ALL 50", Price = 50, DataAllowance = "9GB of data", Freebies = "Unli Calls & Texts", ValidityDays = 7 },
                        new PromoItem { Name = "SURFSAYA 99", Price = 99, DataAllowance = "1.5GB + 100MB/day", Freebies = "Unli Calls & Texts", ValidityDays = 7 },
                        new PromoItem { Name = "ARAW-ARAW DATA 150", Price = 150, DataAllowance = "1GB/day (10.5GB)", Freebies = "10 Mins Calls + 100 Texts", ValidityDays = 7 },
                        new PromoItem { Name = "ALL DATA 299", Price = 299, DataAllowance = "24 GB of data", Freebies = "Unli Calls & Texts", ValidityDays = 30 }
                    };
                case 5: 
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "Level-Up 99 5G", Price = 99, DataAllowance = "14GB 5G of data", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-up 109 5G", Price = 109, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-Up 129 5G", Price = 129, DataAllowance = "20GB of data", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-Up 199 5G", Price = 199, DataAllowance = "32GB of data", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Unli 5G 299", Price = 299, DataAllowance = "Unli 5G + 10GB 4G", Freebies = "Unli AllNet Texts & Calls", ValidityDays = 7 }
                    };
                default:
                    return new List<PromoItem>(); 
            }
        }

        public List<PromoItem> GetRewards()
        {
            return new List<PromoItem>
            {
                new PromoItem { Name = "250MB for 1 Day", DataAllowance = "250MB", Freebies = "None", ValidityDays = 1, Price = 2 },
                new PromoItem { Name = "500MB for 1 Day", DataAllowance = "500MB", Freebies = "None", ValidityDays = 1, Price = 5 },
                new PromoItem { Name = "1GB for 1 Day", DataAllowance = "1GB", Freebies = "None", ValidityDays = 1, Price = 10 },
                new PromoItem { Name = "2GB for 3 Days", DataAllowance = "2GB", Freebies = "None", ValidityDays = 3, Price = 20 },
                new PromoItem { Name = "5GB for 5 Days", DataAllowance = "5GB", Freebies = "None", ValidityDays = 5, Price = 50 },
                new PromoItem { Name = "8GB for 7 Days", DataAllowance = "8GB", Freebies = "None", ValidityDays = 7, Price = 100 }
            };
        }


    }
}

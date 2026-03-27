using System;
using System.Collections.Generic;
using LoadPromoModels;

namespace LoadPromoDataService
{  
    public class LoadPromoData
    {
        public List<string> GetAllNetworks()
        {
            return new List<string> {"TM", "GLOBE", "SMART", "TNT", "DITO"};
        }
        public List<double> GetRegLoadOptions()
        {
            return new List<double> {20, 50, 70, 100, 200, 300, 500};
        }
        public List<PromoItem> GetPromosByNetwork(string networkName)
        {
            switch (networkName.ToUpper())
            {
                case "TM":
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "All-Net SURF 30", Price = 30, DataAllowance = "150 MB/day + 750 MB", Freebies = "Unli AllNet Texts", ValidityDays = 2 },
                        new PromoItem { Name = "EasySURF50", Price = 50, DataAllowance = "6GB of data", Freebies = "Unli AllNet Calls and Texts", ValidityDays = 3 },
                        new PromoItem { Name = "EasySURF99", Price = 99, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts", ValidityDays = 7 },
                        new PromoItem { Name = "EasySURF140", Price = 140, DataAllowance = "18GB of data", Freebies = "Unli AllNet Calls and Texts", ValidityDays = 7 },
                        new PromoItem { Name = "TM EasyPlan 150", Price = 150, DataAllowance = "30GB of data", Freebies = "Unli AllNet Calls and Texts", ValidityDays = 15 }
                    };
                case "GLOBE":
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "GoUNLI30", Price = 30, DataAllowance = "100 MB", Freebies = "Unli AllNet Texts", ValidityDays = 2 },
                        new PromoItem { Name = "Go59 for Students", Price = 59, DataAllowance = "5GB + 1GB GoLEARN", Freebies = "Unli AllNet Texts", ValidityDays = 3 },
                        new PromoItem { Name = "GoPLUS99", Price = 99, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts", ValidityDays = 7 },
                        new PromoItem { Name = "GoPLUS129", Price = 129, DataAllowance = "18GB of data", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 7 },
                        new PromoItem { Name = "GoPLUS250", Price = 250, DataAllowance = "30GB of data", Freebies = "15GB for apps", ValidityDays = 15 }
                    };
                case "SMART":
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "SURFSAYA 35", Price = 35, DataAllowance = "500MB/day + 900MB", Freebies = "Unli AllNet Texts", ValidityDays = 3 },
                        new PromoItem { Name = "POWER ALL 59", Price = 59, DataAllowance = "5GB + 3GB 5G", Freebies = "Unli AllNet Calls and Texts", ValidityDays = 3 },
                        new PromoItem { Name = "SAYA ALL 99", Price = 99, DataAllowance = "6GB + UNLI Apps", Freebies = "Unli Calls and Texts", ValidityDays = 7 },
                        new PromoItem { Name = "NON-STOP DATA 115", Price = 115, DataAllowance = "Unli 5G + NON-STOP 4G", Freebies = "Unli Calls and Texts", ValidityDays = 3 },
                        new PromoItem { Name = "Giga Data 349", Price = 349, DataAllowance = "16GB Data", Freebies = "Unli Calls and Texts", ValidityDays = 30 }
                    };
                case "TNT":
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "SURFSAYA 49", Price = 49, DataAllowance = "1.2GB + 250MB/day", Freebies = "Unli Calls and Texts", ValidityDays = 3 },
                        new PromoItem { Name = "SAYA ALL 50", Price = 50, DataAllowance = "9GB of data", Freebies = "Unli Calls and Texts", ValidityDays = 7 },
                        new PromoItem { Name = "SURFSAYA 99", Price = 99, DataAllowance = "1.5GB + 100MB/day", Freebies = "Unli Calls and Texts", ValidityDays = 7 },
                        new PromoItem { Name = "ARAW-ARAW DATA 150", Price = 150, DataAllowance = "1GB/day (10.5GB)", Freebies = "10 Mins Calls + 100 Texts", ValidityDays = 7 },
                        new PromoItem { Name = "ALL DATA 299", Price = 299, DataAllowance = "24 GB of data", Freebies = "Unli Calls and Texts", ValidityDays = 30 }
                    };
                case "DITO":
                    return new List<PromoItem>
                    {
                        new PromoItem { Name = "Level-Up 99 5G", Price = 99, DataAllowance = "14GB 5G of data", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-up 109 5G", Price = 109, DataAllowance = "16GB of data", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-Up 129 5G", Price = 129, DataAllowance = "20GB of data", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Level-Up 199 5G", Price = 199, DataAllowance = "32GB of data", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 30 },
                        new PromoItem { Name = "Unli 5G 299", Price = 299, DataAllowance = "Unli 5G + 10GB 4G", Freebies = "Unli AllNet Texts and Calls", ValidityDays = 7 }
                    };
                default:
                    return new List<PromoItem>();
            }
        }

    }

}


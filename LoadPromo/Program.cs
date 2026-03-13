using System;
using System.Collections.Generic;
using LoadPromoAppService;
using LoadPromoModels;

namespace LoadPromo
{
    internal class Program
    {
        static TransactionService service = new TransactionService();
        static void Main(string[] args)
        {
            RegisterDefaultAccount();

            while (true)
            {
                Console.WriteLine("\n====== AVAIL SERVICES ======");
                Console.WriteLine("1. Cash-In (Top-Up Wallet)");
                Console.WriteLine("2. Buy Regular Load");
                Console.WriteLine("3. Buy Load Promo");
                Console.WriteLine("4. Account Status");
                Console.WriteLine("5. Transaction History");
                Console.WriteLine("6. Redeem Rewards/Points");
                Console.WriteLine("7. Exit");
                Console.Write("Select: ");
                int choice = GetValidMenuChoice(7);

                switch (choice)
                {
                    case 1:
                        TopUp();
                        break;
                    case 2:
                        RegularLoad();
                        break;
                    case 3:
                        PromoLoad();
                        break;
                    case 4:
                        AccountStatus();
                        break;
                    case 5:
                        ShowHistory();
                        break;
                    case 6:
                        RedeemPoints();
                        break;
                    case 7:
                        Console.WriteLine("\nExiting...");
                        return;
                }
            }
        }

        static void RegisterDefaultAccount()
        {
            Console.WriteLine("===== Load Promo System =====");

            List<string> networks = service.GetNetworks();

            for (int i = 0; i < networks.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + networks[i]);
            }

            Console.Write("Select your default SIM Network: ");
            int netId = GetValidMenuChoice(5);

            Console.Write("Enter your default mobile number: (+63)");
            string phone = GetValidPhoneNumber().ToString();

            service.RegisterUser(networks[netId - 1], netId, phone);

            Console.WriteLine("\nNetwork      : " + networks[netId - 1] +
                              "\nMobile No.   : 0" + phone +
                              "\nRegistration Successful!");
            Console.WriteLine("\nPress Enter to Procced");
            Console.ReadLine();
        }

        static string GetValidPhoneNumber()
        {
            while (true)
            {
                string number = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(number))
                {
                    Console.Write("Mobile number cannot be empty. \n\nPlease enter your number again: (+63)");
                    continue;
                }

                bool isALLDigits = true;
                for (int i = 0; i < number.Length; i++)
                    if (!char.IsDigit(number[i]))
                    {
                        isALLDigits = false;
                        break;
                    }

                if (number.Length == 10 && isALLDigits)
                    return number;
                else
                    Console.Write("Invalid format (+63 XXXXXXXXXX). Must be all digits. \n\nPlease enter your number again: (+63)");
            }
        }

        static void TopUp()
        {
            Console.Write("\nEnter amount to Cash-In: Php ");
            int amount = GetValidNumber();
            string result = service.TopUp(amount);
            Console.WriteLine(result);
        }
            
        static void RegularLoad()
        {
            Account myAcc = service.GetMyAccount();
            int netId = myAcc.NetworkID;
            string phone = myAcc.PhoneNumber;
            string networkName = myAcc.Network;
            bool isMyNum = true;
           
            Console.WriteLine("\n----- Regular Load -----");
            Console.WriteLine("Select Recipient:");
            Console.WriteLine("1. My number (+63)" + phone);
            Console.WriteLine("2. Other number");
            Console.Write("Select: ");
            int userChoice = GetValidMenuChoice(2);

            if (userChoice == 2)
            {;
                isMyNum = false;

                Console.WriteLine("\nNETWORKS");
                List<string> networks = service.GetNetworks();

                for (int i = 0; i < networks.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + networks[i]);
                }
                Console.Write("Select: ");
                netId = GetValidMenuChoice(5);
                networkName = networks[netId - 1];

                isMyNum = false;
                Console.Write("Enter Recipient's Number: (+63)");
                phone = GetValidPhoneNumber();
            }

            List<int> loads = service.GetRegularLoadOptions();

            Console.WriteLine("\nRegular Loads Available");

            for (int i = 0; i < loads.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + loads[i]);
            }

            Console.WriteLine((loads.Count + 1) + ". Other Amount");

            Console.Write("Select: ");
            int choice = GetValidMenuChoice(loads.Count);

            int amount = 0;

            if (choice <= loads.Count)
            {
                amount = loads[choice - 1];
            }
            else
            {
                Console.Write("Enter Custom Load Amount: ");
                amount = GetValidNumber();
            }

            Console.WriteLine("\n==== CONFIRM TRANSACTION ====");
            Console.WriteLine("Recipient     : +63" + phone);
            Console.WriteLine("Load          : " + "Regular " + amount);
            Console.WriteLine("Amount        : " + "Php " + amount);
            Console.WriteLine("Payment Via   : " + "Load Balance");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. BUY LOAD");
            Console.WriteLine("2. Cancel");
            Console.Write("Select: ");
            int confirm = GetValidMenuChoice(2);

            if (confirm == 2)
            {
                Console.WriteLine("Transaction Cancelled");
                return;
            }

            string receipt = service.BuyRegularLoad(amount, phone, isMyNum);
            Console.WriteLine(receipt);
        }

        static void PromoLoad()
            {
            Console.WriteLine("\n---- REGISTER PROMO ----");

            Account myAcc = service.GetMyAccount();
            int netId = myAcc.NetworkID;
            string phone = myAcc.PhoneNumber;
            string networkName = myAcc.Network;
            bool isMyNum = true;

            Console.WriteLine("Select Recipient:");
            Console.WriteLine("1. My number (+63)" + phone);
            Console.WriteLine("2. Other number");
            Console.Write("Select: ");          
            int userChoice = GetValidMenuChoice(2);

            if (userChoice == 2)
            {
                Console.WriteLine("\nNETWORKS");
                isMyNum = false;
                List<string> networks = service.GetNetworks();

                for (int i = 0; i < networks.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + networks[i]);
                }
                Console.Write("Select: ");
                netId = GetValidMenuChoice(5);
                networkName = networks[netId - 1];

                Console.Write("Enter Recipient's Number: (+63)");
                phone = GetValidPhoneNumber();
            }

            List<PromoItem> promos = service.GetPromos(netId);
           
            Console.WriteLine("\nAvailable Promos:");

            for (int i = 0; i < promos.Count; i++)
            {
                string validity = promos[i].ValidityDays == 0 ? "No Expiration" : promos[i].ValidityDays + " Days";
                Console.WriteLine((i + 1) + ". " + promos[i].Name);
            }
                Console.Write("Select Promo: ");
                int choice = GetValidMenuChoice(promos.Count);
                PromoItem selectedPromo = promos[choice - 1];

            Console.WriteLine("\n===== CONFIRM TRANSACTION =====");
            Console.WriteLine("Recipient     : (+63)" + phone);
            Console.WriteLine("Promo         : " + selectedPromo.Name);
            Console.WriteLine("Inclusions    : " + selectedPromo.DataAllowance + " + " + selectedPromo.Freebies);
            Console.WriteLine("Price         : Php " + selectedPromo.Price);
            Console.WriteLine("Validity      : " +  selectedPromo.ValidityDays + " days");
            Console.WriteLine("Payment Via   : " + "Load Balance");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. BUY PROMO");
            Console.WriteLine("2. Cancel");
        
            Console.Write("Select: ");
            int confirm = GetValidMenuChoice(2);

            if (confirm == 2)
            {
                Console.WriteLine("Transaction Cancelled.");
                return;
            }

            string receipt = service.BuyPromoLoad(selectedPromo, phone, isMyNum);
            Console.WriteLine(receipt);              
        }
        
        static void RedeemPoints()
        {
            Account acc = service.GetMyAccount();
            Console.WriteLine("Current Points: " + acc.TotalPoints + " point(s).");

            Console.WriteLine("\n---- REDEEM REWARD POINTS ----");
            Console.WriteLine("Current Points: " + acc.TotalPoints + " point(s).");
            List<PromoItem> rewards = service.GetRewards();

            Console.WriteLine("Available Rewards:");
            for (int i = 0; i < rewards.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {rewards[i].Name}   (Points Required: {rewards[i].Price} pts)");
            }
            Console.WriteLine($"{rewards.Count + 1}. Cancel");
            Console.Write("Select: ");

            int choice = GetValidMenuChoice(rewards.Count + 1);
            if (choice == rewards.Count + 1) return;

            PromoItem selectedReward = rewards[choice - 1];
            int cost = selectedReward.Price;

            Console.WriteLine("\n==== CONFIRM REDEMPTION ====");
            Console.WriteLine("Reward        : " + selectedReward.Name);
            Console.WriteLine("Points        : " + "Cost " + cost + " points");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. REDEEM");
            Console.WriteLine("2. Cancel");
            Console.Write("Select: ");
            if (GetValidMenuChoice(2) == 2)
            {
                Console.WriteLine("Redemption Cancelled.");
                return;
            }

            string result = service.RedeemPoints(selectedReward, cost);
            Console.WriteLine(result); 
        }

        static void AccountStatus()
        {
            Account acc = service.GetMyAccount();
            Console.WriteLine("\n---- ACCOUNT STATUS ----");
            Console.WriteLine("Current Wallet Balance : Php " + acc.WalletBalance);
            Console.WriteLine("SIM Regular Load       : Php " + acc.MySimLoad);
            Console.WriteLine("Total Reward Points    : " + acc.TotalPoints + " point(s)");

            if (acc.ActivePromo != "None")
            {
                Console.WriteLine("\nActive Promo       : " + acc.ActivePromo);
                Console.WriteLine("Data Allowance     : " + acc.ActiveData);
                Console.WriteLine("Valid Until        : " + acc.ActiveExpiry);
            }
            else
            {
                Console.WriteLine("\nPromo Status: You are not registered to any promo.");
            }
        }
        static void ShowHistory()
        {
            Console.WriteLine("\n===== TRANSACTION HISTORY =====");

            List<LoadPromoModels.Transaction> history = service.GetHistory();

            if (history.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
            }

            foreach (var t in history)
            {
                Console.WriteLine($"{t.Date}  |Ref: { t.RefNumber}  |{t.Details}");
            }
        }
     
        static int GetValidMenuChoice(int max)
                {
                    while (true)
                    {
                        string choice = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(choice))
                        {
                            Console.Write("Input cannot be empty. \nSelect" + " (1-" + max + "): ");
                            continue;
                        }

                        bool valid = true;
                        for (int i = 0; i < choice.Length; i++)
                        {
                            if (!char.IsDigit(choice[i]))
                                valid = false;
                        }

                        if (valid && choice.Length > 0)
                        {
                            int num = Convert.ToInt32(choice);
                            if (num >= 1 && num <= max)
                                return num;
                        }
                        Console.Write("Invalid. Please try again. \nSelect" + " (1-" + max + "): ");
                    }
                }

        static int GetValidNumber()
                {
                    while (true)
                    {
                        string input = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(input))
                        {
                            Console.Write("Amount cannot be empty. Please try again, \nEnter amount: Php ");
                            continue;
                        }

                        bool valid = true;
                        for (int i = 0; i < input.Length; i++)
                        {
                            if (!char.IsDigit(input[i]))
                                valid = false;
                        }

                        if (valid && input.Length > 0)
                        {
                            return Convert.ToInt32(input);

                        }
                        Console.Write("Invalid. Please input a positive number only. \nEnter amount: Php ");
                    }
                }
           
      }
}

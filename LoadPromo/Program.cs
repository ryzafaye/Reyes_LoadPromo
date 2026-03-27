using System;
using System.Collections.Generic;
using System.Numerics;
using LoadPromoAppService;
using LoadPromoModels;

namespace LoadPromo
{
    internal class Program
    {
        static TransactionService service = new TransactionService();
        static void Main(string[] args)
        {
            while (true)
            {
                RegisterDefaultAccount();
                bool isLoggedIn = true;

                while (isLoggedIn)
                {
                    Account acc = service.GetMyAccount();
                    Console.WriteLine("\n======= AVAIL SERVICES =======");
                    Console.WriteLine("Load Balance: " + acc.SimLoadBalance);
                    Console.WriteLine("[1] Top-Up Wallet");
                    Console.WriteLine("[2] Regular Load");
                    Console.WriteLine("[3] Load Promo");
                    Console.WriteLine("[4] Account Status");
                    Console.WriteLine("[5] Transaction History");
                    Console.WriteLine("[6] Switch Number");
                    Console.WriteLine("[7] Exit");
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
                            History();
                            break;
                        case 6:
                            isLoggedIn = false;
                            break;
                        case 7:
                            Console.WriteLine("\nExiting...");
                            return;
                    }
                }
            }
        }
        static void RegisterDefaultAccount()
        {
            Console.WriteLine("\n======= Load Promo System =======");

            Console.Write("\nEnter your mobile number: (+63)");
            string phone = GetValidPhoneNumber().ToString();

            if (service.LoadAccountIfExists(phone))
            {
                Console.WriteLine("\nWelcome back, " + phone +"!");
            }
            else
            {
                List<string> networks = service.GetNetworks();

                for (int i = 0; i < networks.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + networks[i]);
                }

                Console.Write("Select your SIM Network: ");
                int choice = GetValidMenuChoice(networks.Count);
                string selectedNetwork = networks[choice - 1];

                service.RegisterUser(selectedNetwork, phone);

                Console.WriteLine("\nNetwork      : " + selectedNetwork +
                                    "\nMobile No.   : " + phone +
                                    "\nRegistration Successful!");
                Console.WriteLine("\nPress Enter to Procced");
                Console.ReadLine();
            }               
        }                   
        static void TopUp()
        {
            Console.Write("\nEnter amount to Cash-In: Php ");
            double amount = GetValidNumber();
            TransactionResponse response = service.TopUp(amount);

            if (response.ResultStatus == Status.InvalidAmount)
            {
                Console.WriteLine("\nTransaction Failed: Minimum amount is Php 5.");
                return;
            }

            Account acc = service.GetMyAccount();
            Console.WriteLine("\nWallet topped up sucessfully! \nNew Balance: Php " + acc.WalletBalance);
        }
        static void RegularLoad()
        {
            Console.WriteLine("\n======= Regular Load =======");

            Account acc = service.GetMyAccount();

            List<double> loads = service.GetRegularLoadOptions();
            Console.WriteLine("\nRegular Loads Available");

            for (int i = 0; i < loads.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + loads[i].ToString("0"));
            }

            Console.WriteLine((loads.Count + 1) + ". Other Amount");
            Console.Write("Select: ");
            int choice = GetValidMenuChoice(loads.Count + 1);

            double amount = 0.0;
            if (choice <= loads.Count)
            {
                amount = loads[choice - 1];
            }
            else
            {
                Console.Write("Enter Custom Load Amount: ");
                amount = GetValidNumber();
            }

            Console.WriteLine("\n===== CONFIRM TRANSACTION ======");
            Console.WriteLine("Recipient     : " + acc.PhoneNumber);
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
                Console.WriteLine("\nTransaction Cancelled");
                return;
            }

            TransactionResponse response = service.BuyRegularLoad(amount);

            if (response.ResultStatus == Status.InsufficientBalance)
            {
                Console.WriteLine("\nInsufficient Wallet Balance. Please Top-Up first.");
                return;
            }

            acc = service.GetMyAccount();
            string expiry = DateTime.Now.AddDays(365).ToString("MM/dd/yyyy hh:mm tt");

            Console.WriteLine($"\n-----------------------------------------\n" +
                $"           NEW MESSAGE RECEIVED          \n" +
                $"-----------------------------------------\n" +
                $"\n{response.ReceiptData.Date}\n\nCongratulations, {acc.PhoneNumber}! You have successfully loaded Regular {amount}. \nValid Until {expiry}. Remaining Load Balance: {acc.WalletBalance} \n\nRef No. {response.ReceiptData.ReferenceNumber}\n-----------------------------------------\n");
        }
        static void PromoLoad()
        {
            Console.WriteLine("\n======= REGISTER PROMO =======");

            Account acc = service.GetMyAccount();
            List<PromoItem> promos = service.GetPromos(acc.Network);

            Console.WriteLine("\nAvailable Promos:");

            for (int i = 0; i < promos.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + promos[i].Name);
            }
            Console.Write("Select Promo: ");
            int choice = GetValidMenuChoice(promos.Count);
            PromoItem selectedPromo = promos[choice - 1];

            acc = service.GetMyAccount();

            Console.WriteLine("\n===== PAYMENT METHOD =====");
            Console.WriteLine($"1. Wallet Balance   (Current: Php {acc.WalletBalance})");
            Console.WriteLine($"2. Load Balance     (Current: {acc.SimLoadBalance})");
            Console.WriteLine("3. Cancel");
            Console.Write("Select Payment Method: ");
            int paymentChoice = GetValidMenuChoice(3);

            if (paymentChoice == 3)
            {
                Console.WriteLine("\nTransaction Cancelled.");
                return;
            }
            string paymentName = (paymentChoice == 1) ? "Wallet Balance" : "Load Balance";

            Console.WriteLine("\n===== CONFIRM TRANSACTION =====");
            Console.WriteLine("Recipient     : " + acc.PhoneNumber);
            Console.WriteLine("Promo         : " + selectedPromo.Name);
            Console.WriteLine("Inclusions    : " + selectedPromo.DataAllowance + " + " + selectedPromo.Freebies);
            Console.WriteLine("Price         : Php " + selectedPromo.Price);
            Console.WriteLine("Validity      : " + selectedPromo.ValidityDays + " days");
            Console.WriteLine("Payment Via   : " + paymentName);
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. BUY PROMO");
            Console.WriteLine("2. Cancel");
            Console.Write("Select: ");
            int confirm = GetValidMenuChoice(2);

            if (confirm == 2)
            {
                Console.WriteLine("\nTransaction Cancelled.");
                return;
            }

            TransactionResponse response = service.BuyPromoLoad(selectedPromo, paymentChoice);

            if (response.ResultStatus == Status.InsufficientBalance)
            {
                Console.WriteLine("\nInsufficient " + paymentName +" Please Top-Up first to avail this promo.");
                return;
            }

            string expiry = DateTime.Now.AddDays(selectedPromo.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");
            acc = service.GetMyAccount();
            double remainingBalance = (paymentChoice == 1) ? acc.WalletBalance : acc.SimLoadBalance;

            Console.WriteLine($"\n-----------------------------------------\n" +
                $"           NEW MESSAGE RECEIVED          \n" +
                $"-----------------------------------------" +
                $"\n{response.ReceiptData.Date}\n\nCongratulations, {acc.PhoneNumber}! You have successfully registered to {selectedPromo.Name}. \nEnjoy {selectedPromo.DataAllowance} + {selectedPromo.Freebies}. \nValid until {expiry}. Remaining {paymentName}: {remainingBalance} \n\nRef No. {response.ReceiptData.ReferenceNumber}\n-----------------------------------------");
        }
        static void AccountStatus()
        {
            Account acc = service.GetMyAccount();
            Console.WriteLine("\n======= ACCOUNT STATUS =======");
            Console.WriteLine("Current Wallet Balance : Php " + acc.WalletBalance);
            Console.WriteLine("SIM Regular Load       : " + acc.SimLoadBalance);
            Console.WriteLine("Valid Until            : " + acc.SimLoadExpiry);

            if (!string.IsNullOrEmpty(acc.ActivePromo))
            {
                Console.WriteLine("\nActive Promo           : " + acc.ActivePromo);
                Console.WriteLine("Data Allowance         : " + acc.ActiveData);
                Console.WriteLine("Valid Until            : " + acc.ActiveExpiry);

                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Cancel Promo Subscription");
                Console.WriteLine("2. Back to Main Menu");
                Console.Write("Select: ");
                int choice = GetValidMenuChoice(2);

                if (choice == 1)
                {
                    Console.WriteLine("\nWARNING: Stopping this promo will forfeit your remaining data and freebies.");
                    Console.WriteLine("Are you sure you want to unsubscribe?");
                    Console.WriteLine("1. YES");
                    Console.WriteLine("2. Cancel");
                    Console.Write("Select: ");
                    int confirm = GetValidMenuChoice(2);

                    if (confirm == 1)
                    {
                        TransactionResponse response = service.CancelActivePromo();

                        Console.WriteLine($"\n-----------------------------------------\n" +
                           $"            NEW MESSAGE RECEIVED          \n" +
                           $"-----------------------------------------" +
                           $"\n{response.ReceiptData.Date}\n\nYou have successfully unsubscribed from {acc.ActivePromo}. All remaining data has been forfeited.\n\nRef No. {response.ReceiptData.ReferenceNumber}\n-----------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Unsubscription cancelled. Your promo is still active.");
                    }
                }
            }
            else
            {
                Console.WriteLine("\nPromo Status: You are not registered to any promo.");
                Console.WriteLine("\nPress Enter to return to Main Menu...");
                Console.ReadLine();
            }
        }
        static void History()
            {
                Console.WriteLine("\n======= TRANSACTION HISTORY =======");

                List<LoadPromoModels.Transaction> history = service.GetHistory();

                if (history.Count == 0)
                {
                    Console.WriteLine("No transactions yet.");
                }

                foreach (var t in history)
                {
                Console.WriteLine($"{t.Date} | Ref No.: {t.ReferenceNumber,-6} | {t.LoadType,-20} | Amount: Php {t.Amount}");
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

                if (int.TryParse(choice, out int num) && num >= 1 && num <= max)
                {
                    return num; 
                }
                Console.Write("Invalid. Please enter a number within the range. \nSelect" + " (1-" + max + "): ");
                }
            }
        static double GetValidNumber()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("Amount cannot be empty. Please try again, \nEnter amount: Php ");
                    continue;
                }

                if (double.TryParse(input, out double amount) && amount > 0)
                {
                    return amount;
                }
                Console.Write("Invalid. Please input a positive number only. \nEnter amount: Php ");
                }
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
                    return "0" + number;

                else
                    Console.Write("Invalid format (+63 XXXXXXXXXX). Enter you 10-digit number. \n\nPlease enter your number again: (+63)");
            }
        }
    }
}

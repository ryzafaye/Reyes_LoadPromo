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
                bool isLoggedIn = StartMenu(); ;

                while (isLoggedIn)
                {
                    Account acc = service.GetMyAccount();

                    Message("\n========  AVAIL SERVICES  ========", ConsoleColor.Cyan);
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("Wallet Balance: " + acc.WalletBalance);
                    Console.WriteLine("Load Balance: " + acc.SimLoadBalance);
                    Console.WriteLine("\n[1] Top-Up Wallet");
                    Console.WriteLine("[2] Regular Load");
                    Console.WriteLine("[3] Load Promo");
                    Console.WriteLine("[4] Account Status");
                    Console.WriteLine("[5] Transaction History");
                    Console.WriteLine("[6] Logout");
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
                            Console.WriteLine("\nLogging out...");
                            break;
                        case 7:
                            Console.WriteLine("\nExiting...");
                            return;
                    }
                }
            }
        }

        static bool StartMenu()
        {
            while (true)
            {
                Message("\n======== LOAD PROMO SYSTEM ========", ConsoleColor.Cyan);
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("1. Register New Account");
                Console.WriteLine("2. Login");
                Console.WriteLine("-----------------------------------");
                Console.Write("Select: ");
                int choice = GetValidMenuChoice(2);

                if (choice == 1)
                {
                    return Register();
                }
                else if (choice == 2)
                {  
                    return Login();
                }
            }
        }

        static bool Register()
        {
            Message("\n=======  REGISTER ACCOUNT  ========", ConsoleColor.Cyan);
            Console.Write("Enter your mobile number: (+63)");
            string phone = GetValidPhoneNumber();

            if (service.IsAccountRegistered(phone))
            {
                Message("\nThis number is already registered! Please go to Login.", ConsoleColor.Red);
                return false;
            }

            Console.Write("Create a 4-Digit PIN: ");
            string pin = GetValidPin();

            List<string> networks = service.GetNetworks();
            Console.WriteLine("\nSIM Networks:");
            for (int i = 0; i < networks.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + networks[i]);
            }
            Console.WriteLine("----------");
            Console.Write("Select: ");
            int choice = GetValidMenuChoice(networks.Count);
            string selectedNetwork = networks[choice - 1];

            service.RegisterUser(selectedNetwork, phone, pin);

            Console.WriteLine("\nNetwork      : " + selectedNetwork +
                              "\nMobile No.   : " + phone);
            Message("\nRegistration Successful!", ConsoleColor.Green);
            return true;
        }      

        static bool Login()
        {
            Message("\n=============  LOGIN  =============", ConsoleColor.Cyan);
            Console.Write("Enter your mobile number: (+63)");
            string phone = GetValidPhoneNumber();

            if (!service.IsAccountRegistered(phone))
            {
                Message("\nAccount not found! Please Register first.", ConsoleColor.Red);
                return false;
            }

            int attempts = 3;
            while (attempts > 0)
            {
                Console.Write($"Enter 4-Digit PIN: ");
                string pin = GetValidPin();

                if (service.Login(phone, pin))
                {
                    Message("\nLogin Successful!", ConsoleColor.Green);
                    Console.WriteLine("\nWelcome back, "+ phone);
                    return true;
                }

                attempts--;
                Message("Incorrect PIN.", ConsoleColor.Red);
            }
            Message("\nToo many failed attempts. Returning to main menu.", ConsoleColor.Red);
            return false;
        }
 
        static void TopUp()
        {
            Message("\n========== TOP-UP WALLET ==========", ConsoleColor.Cyan);
            Console.Write("\nEnter amount to Cash-In: Php ");
            double amount = GetValidNumber();
            TransactionResponse response = service.TopUp(amount);

            if (response.ResultStatus == Status.InvalidAmount)
            {
                Message("\nTransaction Failed: Minimum amount is Php 5.", ConsoleColor.Red);
                return;
            }

            Account acc = service.GetMyAccount();
            Message("\nWallet topped-up sucessfully!", ConsoleColor.Green);
            Console.WriteLine("\nNew Balance: Php " + acc.WalletBalance);
        }

        static void RegularLoad()
        {
            Message("\n=========== REGULAR LOAD ==========", ConsoleColor.Cyan);

            Account acc = service.GetMyAccount();
            string receiverPhone = GetRecipientNumber(acc.PhoneNumber);

            List<double> loads = service.GetRegularLoadOptions();
            Console.WriteLine("\nRegular Loads Available");

            for (int i = 0; i < loads.Count; i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + loads[i].ToString("0"));
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
            Console.WriteLine("Recipient     : " + receiverPhone);
            Console.WriteLine("Load          : " + "Regular " + amount);
            Console.WriteLine("Amount        : " + "Php " + amount);
            Console.WriteLine("Payment Via   : " + "Wallet Balance");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("[1] BUY LOAD");
            Console.WriteLine("[2] Cancel");
            Console.Write("Select: ");
            int confirm = GetValidMenuChoice(2);

            if (confirm == 2)
            {
                Message("\nTransaction Cancelled", ConsoleColor.Red);
                return;
            }

            TransactionResponse response = service.BuyRegularLoad(amount, receiverPhone);

            if (response.ResultStatus == Status.InsufficientBalance)
            {
                Message("\nInsufficient Wallet Balance. Please Top-Up first.", ConsoleColor.Red);
                return;
            }
            else if (response.ResultStatus == Status.Error)
            {
                Message("\nTransaction Failed: Number not found.", ConsoleColor.Red);
                return;
            }

            acc = service.GetMyAccount();
            string expiry = DateTime.Now.AddDays(365).ToString("MM/dd/yyyy hh:mm tt");

            Console.WriteLine($"\n-----------------------------------------\n" +
                $"           NEW MESSAGE RECEIVED          \n" +
                $"-----------------------------------------\n" +
                $"{response.ReceiptData.Date}\n\nCongratulations! You have successfully loaded Regular {amount} to {receiverPhone}. \nValid Until {expiry}. Remaining Wallet Balance: {acc.WalletBalance} \n\nRef No. {response.ReceiptData.ReferenceNumber}\n-----------------------------------------\n");
        }

        static void PromoLoad()
        {
            Message("\n============ BUY PROMO ============", ConsoleColor.Cyan);

            Account acc = service.GetMyAccount();
            string receiverPhone = GetRecipientNumber(acc.PhoneNumber);

            string targetNetwork = service.GetAccountInfo(receiverPhone).Network;

            List<PromoItem> promos = service.GetPromos(targetNetwork);
            Console.WriteLine("\nAvailable Promos:");

            for (int i = 0; i < promos.Count; i++)
            {
                Console.WriteLine("[" + (i + 1) + "] " + promos[i].Name);
            }
            Console.Write("Select Promo: ");
            int choice = GetValidMenuChoice(promos.Count);
            PromoItem selectedPromo = promos[choice - 1];

            Console.WriteLine("\n" + selectedPromo.Name);
            Console.WriteLine($"\nEnjoy {selectedPromo.DataAllowance} and {selectedPromo.Freebies}. \nValid for {selectedPromo.ValidityDays} days for only Php {selectedPromo.Price}.");
            Console.WriteLine("---------------------------");
            Console.WriteLine("[1] Proceed to Payment");
            Console.WriteLine("[2] Cancel");
            Console.Write("Select: ");

            if (GetValidMenuChoice(2) == 2)
            {
                Message("\nTransaction Cancelled.", ConsoleColor.DarkYellow);
                return;
            }

            acc = service.GetMyAccount();

            Console.WriteLine("\n===== PAYMENT METHOD =====");
            Console.WriteLine("Amount: Php " + selectedPromo.Price);
            Console.WriteLine($"[1] Wallet Balance   (Current: Php {acc.WalletBalance})");
            Console.WriteLine($"[2] Load Balance     (Current: {acc.SimLoadBalance})");
            Console.WriteLine("[3] Cancel");
            Console.Write("Select Payment Method: ");
            int paymentChoice = GetValidMenuChoice(3);

            if (paymentChoice == 3)
            {
                Message("\nTransaction Cancelled.", ConsoleColor.DarkYellow);
                return;
            }
            string paymentName = (paymentChoice == 1) ? "Wallet Balance" : "Load Balance";

            Console.WriteLine("\n===== CONFIRM TRANSACTION =====");
            Console.WriteLine("Recipient     : " + receiverPhone);
            Console.WriteLine("Promo         : " + selectedPromo.Name);
            Console.WriteLine("Inclusions    : " + selectedPromo.DataAllowance + " + " + selectedPromo.Freebies);
            Console.WriteLine("Validity      : " + selectedPromo.ValidityDays + " days");
            Console.WriteLine("Payment Via   : " + paymentName);
            Console.WriteLine("Total         : Php " + selectedPromo.Price);
            Console.WriteLine("-------------------------------");
            Console.WriteLine("[1] BUY PROMO");
            Console.WriteLine("[2] Cancel");
            Console.Write("Select: ");
            int confirm = GetValidMenuChoice(2);

            if (confirm == 2)
            {
                Message("\nTransaction Cancelled.", ConsoleColor.DarkYellow);
                return;
            }

            TransactionResponse response = service.BuyPromoLoad(selectedPromo, paymentChoice, receiverPhone);

            if (response.ResultStatus == Status.InsufficientBalance)
            {
                Message("\nInsufficient " + paymentName +". Please Top-Up first to avail this promo.", ConsoleColor.Red);
                return;
            }
            else if (response.ResultStatus == Status.Error)
            {
                Message("\nTransaction Failed: Number not found.", ConsoleColor.Red);
                return;
            }

            string expiry = DateTime.Now.AddDays(selectedPromo.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");
            acc = service.GetMyAccount();
            double remainingBalance = (paymentChoice == 1) ? acc.WalletBalance : acc.SimLoadBalance;

            Console.WriteLine($"\n-----------------------------------------\n" +
                $"           NEW MESSAGE RECEIVED          \n" +
                $"-----------------------------------------\n" +
                $"{response.ReceiptData.Date}\n\nCongrats! You have successfully subscribed {receiverPhone} to {selectedPromo.Name}. \nYou can now enjoy {selectedPromo.DataAllowance} + {selectedPromo.Freebies}. \nValid until {expiry}. Remaining {paymentName}: {remainingBalance} \n\nRef No. {response.ReceiptData.ReferenceNumber}\n-----------------------------------------");
        }

        static void AccountStatus()
        {
            Account acc = service.GetMyAccount();

            Message("\n========= ACCOUNT STATUS ==========", ConsoleColor.Cyan);

            Console.WriteLine("\nCurrent Wallet Balance : Php " + acc.WalletBalance);
            Console.WriteLine("SIM Regular Load       : " + acc.SimLoadBalance);
            Console.WriteLine("Valid Until            : " + acc.SimLoadExpiry);

            if (!string.IsNullOrEmpty(acc.ActivePromo))
            {
                Console.WriteLine("\nActive Promo           : " + acc.ActivePromo);
                Console.WriteLine("Data Allowance         : " + acc.ActiveData);
                Console.WriteLine("Valid Until            : " + acc.ActiveExpiry);

                Console.WriteLine("\nOptions:");
                Console.WriteLine("[1] Cancel Promo Subscription");
                Console.WriteLine("[2] Back to Main Menu");
                Console.Write("Select: ");
                int choice = GetValidMenuChoice(2);

                if (choice == 1)
                {
                    Console.WriteLine("\nWARNING: Stopping this promo will forfeit your remaining data and freebies.");
                    Console.WriteLine("Are you sure you want to unsubscribe?");
                    Console.WriteLine("[1] YES");
                    Console.WriteLine("[2] Cancel");
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
                        Message("Unsubscription cancelled. Your promo is still active.", ConsoleColor.Green);
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
            Message("\n======= TRANSACTION HISTORY =======", ConsoleColor.Cyan);

            List<LoadPromoModels.Transaction> history = service.GetHistory();

            if (history.Count == 0)
            {
                Console.WriteLine("\nNo transactions yet.");
            }

            foreach (var t in history)
            {
            Console.WriteLine($"\n{t.Date} | Ref No.: {t.ReferenceNumber,-6} | Recipient: {t.Recipient,-10} | {t.LoadType,-16} | Amount: Php {t.Amount}");
            }
        }

        static string GetValidPin()
        {
            while (true)
            {
                string pin = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(pin) && pin.Length == 4 && int.TryParse(pin, out _))
                {
                    return pin;
                }
                Message("Invalid PIN. Please enter a 4-digit number.", ConsoleColor.Red);
                Console.Write("\nEnter your PIN: ");
            }
        }

        static string GetRecipientNumber(string myNumber)
        {
            Console.WriteLine("Buy Load For:");
            Console.WriteLine("\n[1] My Number (" + myNumber + ")");
            Console.WriteLine("[2] Other Number");
            Console.Write("Select: ");
            int targetChoice = GetValidMenuChoice(2);

            if (targetChoice == 1) return myNumber;

            while (true)
            {
                Console.Write("\nEnter the mobile number of the recipient: (+63)");
                string phone = GetValidPhoneNumber();

                if (service.IsAccountRegistered(phone))
                {
                    return phone; 
                }

                Message("\nNumber not found. Please try again.", ConsoleColor.Red);
            }
        }

        static int GetValidMenuChoice(int max)
        {
            while (true)
            {
                string choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    Message("Input cannot be empty.", ConsoleColor.Red);
                    Console.Write("\nSelect" + "(1-" + max + "): ");
                    continue;
                }

                if (int.TryParse(choice, out int num) && num >= 1 && num <= max)
                {
                    return num; 
                }
                Message("Invalid. Please choose an option within the range.", ConsoleColor.Red);
                Console.Write("\nSelect" + "(1-" + max + "): ");
            }
        }

        static double GetValidNumber()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Message("Amount cannot be empty. Please enter an amount.", ConsoleColor.Red);
                    Console.Write("\nEnter amount: Php ");
                    continue;
                }

                if (double.TryParse(input, out double amount) && amount > 0)
                {
                    return amount;
                }
                Message("Invalid. Please input a positive number only.", ConsoleColor.Red);
                Console.Write("\nEnter amount: Php ");
            }
        }

        static string GetValidPhoneNumber()
        {
            while (true)
            {
                string number = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(number))
                {
                    Message("Mobile number cannot be empty.", ConsoleColor.Green);
                    Console.Write("\n\nPlease enter your number again: (+63)");
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
                {
                    return "0" + number;
                }
                else
                { 
                    Message("Invalid format (+63 XXXXXXXXXX). Enter you 10-digit number.", ConsoleColor.Red);
                    Console.Write("\n\nPlease enter your number again: (+63)");
                }
            }
        }

        static void Message(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}

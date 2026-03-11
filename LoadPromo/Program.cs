using System;
using System.Collections.Generic;


namespace LoadPromos
{
    internal class Program
    {
        static int balance = 0;
        static double totalPoints = 0.0;

        static string ownerNumber = "";
        static string ownerNetwork = "";
        static int ownerNetworkID = 0;

        static int mySimLoad = 0;

        static string activePromo = "none";
        static string activeData = "none";
        static string activeExpiry = "none";
        static string activeFreebies = "none";

        static List<string> history = new List<string>();
        static int transactionNumber = 1;
        static void Main(string[] args)
        {
            RegisterDefaultAccount();

            while (true)
            {
                Console.WriteLine("\n====== AVAIL SERVICES ======");
                Console.WriteLine("1. Cash-In (Top-Up Wallet)");
                Console.WriteLine("2. Buy Regular Load");
                Console.WriteLine("3. Buy Load Promo");
                Console.WriteLine("4. Check Balance");
                Console.WriteLine("5. Check Promo Status");
                Console.WriteLine("6. Transaction History");
                Console.WriteLine("7. Redeem Rewards/Points");
                Console.WriteLine("8. Exit");

                Console.Write("Choose: ");
                int choice = GetValidMenuChoice(9);

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
                        CheckBalance();
                        break;
                    case 5:
                        PromoStatus();
                        break;
                    case 6:
                        ShowHistory();
                        break;
                    case 7:
                        RedeemPoints();
                        break;
                    case 8:
                        Console.WriteLine("\nExiting...");
                        return;
                }
            }
        }

            static void RegisterDefaultAccount()
            {
                Console.WriteLine("===== Load Promo System =====");

                Console.WriteLine("Select your default SIM Network: ");
                ownerNetworkID = SelectNetwork();
                ownerNetwork = NetworkName(ownerNetworkID);

                Console.WriteLine("\nEnter your default mobile number.");
                ownerNumber = GetValidPhoneNumber();

                Console.WriteLine("\nRegistration Successful!");
                Console.WriteLine("Default Network : " + ownerNetwork);
                Console.WriteLine("Default Number  : +63" + ownerNumber);
                Console.WriteLine("\nPress Enter to Procced");
                Console.ReadLine();
            }

            static string NetworkName(int choice)
            {
                string[] networks = { "TM", "GLOBE", "SMART", "TNT", "DITO" };
                return networks[choice - 1];
            }

            static int SelectNetwork()
            {
                List<string> networks = new List<string> { "TM", "GLOBE", "SMART", "TNT", "DITO" };

                for (int i = 0; i < networks.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + networks[i]);
                }

                Console.Write("Choose: ");
                return GetValidMenuChoice(networks.Count);
            }

            static string GetValidPhoneNumber()
            {
                while (true)
                {
                    Console.Write("Mobile number: +63");
                    string number = Console.ReadLine();

                    if (number == null || number.Length == 0)
                    {
                        Console.WriteLine("Mobile number cannot be empty. Please enter your number again.");
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
                        Console.WriteLine("Invalid format. (+639 XXXXXXXXX) Must be all digits.");
                }
            }

            static void TopUp()
            {
                Console.Write("\nEnter amount to Cash-In: Php ");
                int amount = GetValidNumber();

                if (amount < 1)
                {
                    Console.WriteLine("Minimum Cash-in is Php 1.");
                    return;
                }

                balance += amount;
                Console.WriteLine("\nWallet topped up successfully!");
                Console.WriteLine("New Wallet Balance: Php " + balance);

                SaveHistory("Cashed-in Php " + amount + " to wallet.");
            }

            static void RegularLoad()
            {
                Console.WriteLine("\n----- Regular Load -----");
                Console.WriteLine("Select Recipient:");
                Console.WriteLine("1. My number (" + ownerNetwork + " - " + "+63" + ownerNumber + ")");
                Console.WriteLine("2. Other number");
                Console.Write("Choose: ");
                int userChoice = GetValidMenuChoice(2);

                string networkName = "";
                string phone = "";

                if (userChoice == 1)
                {
                    networkName = ownerNetwork;
                    phone = ownerNumber;
                }
                else
                {
                    Console.WriteLine("Select Network");
                    int networkID = SelectNetwork();
                    networkName = NetworkName(networkID);

                    Console.WriteLine("Enter Recipient's Number");
                    phone = GetValidPhoneNumber();
                }

                int[] loads = { 20, 50, 100, 200, 300, 500 };

                Console.WriteLine("\nRegular Loads Available");

                for (int i = 0; i < loads.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + loads[i]);
                }

                Console.WriteLine((loads.Length + 1) + ". Other Amount");

                Console.Write("Select: ");
                int choice = GetValidMenuChoice(loads.Length + 1);

                int amount = 0;

                if (choice <= loads.Length)
                {
                    amount = loads[choice - 1];
                }
                else
                {
                    Console.Write("Enter Custom Amount: ");
                    amount = GetValidNumber();

                    if (amount < 10)
                    {
                        Console.WriteLine("Miminum regular load amount is Php 10");
                        return;
                    }
                }

                Console.WriteLine("\n==== CONFIRM TRANSACTION ====");
                Console.WriteLine("Mobile Number : +63" + phone);
                Console.WriteLine("Load          : " + "Regular " + amount);
                Console.WriteLine("Amount        : " + "Php " + amount);
                Console.WriteLine("Payment Via   : " + "Wallet Balance");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. BUY LOAD");
                Console.WriteLine("2. Cancel");
                Console.Write("Choose: ");
                int confirm = GetValidMenuChoice(2);

                if (confirm == 2)
                {
                    Console.WriteLine("Transaction Cancelled");
                    return;
                }
                if (balance < amount)
                {
                    Console.WriteLine("Insufficient Balance. Please Top-Up your Wallet.");
                    return;
                }

                balance -= amount;
                double pointsEarned = amount / 100.0;
                totalPoints += pointsEarned;

                if (userChoice == 1)
                {
                    mySimLoad += amount;
                }

                string regLoadExpiry = DateTime.Now.AddDays(365).ToString("MM/dd/yyyy hh:mm tt");

                ConfirmationMessage(phone, "Regular Load", amount, "Regular", pointsEarned, regLoadExpiry);
                SaveHistory("Regular Load Php " + amount + "to " + phone);

            }

            static void PromoLoad()
            {
                Console.WriteLine("\n---- REGISTER PROMO ----");

                Console.WriteLine("Select Recipient:");
                Console.WriteLine("1. My Number (" + ownerNetwork + " - " + ownerNumber + ")");
                Console.WriteLine("2. Other Number");
                Console.Write("Choose: ");
                int targetChoice = GetValidMenuChoice(2);

                int networkID = 0;
                string networkName = "";
                string phone = "";

                if (targetChoice == 1)
                {
                    networkID = ownerNetworkID;
                    networkName = ownerNetwork;
                    phone = ownerNumber;
                }
                else
                {
                    Console.WriteLine("\nSelect Network for Other Number:");
                    networkID = SelectNetwork();
                    networkName = NetworkName(networkID);

                    Console.WriteLine("Enter Recipient's Number:");
                    phone = GetValidPhoneNumber();
                }

                string[] promoNames = new string[0];
                int[] promoPrices = new int[0];
                string[] promoData = new string[0];
                string[] promoFreebies = new string[0];
                int[] promoDays = new int[0];

                switch (networkID)
                {
                    case 1:
                        promoNames = new string[] { "All-Net SURF 20", "All-Net SURF 30", "EasySURF50", "EasySURF75", "EasySURF99", "EasySURF140", "TM EasyPlan 150" };
                        promoPrices = new int[] { 20, 30, 50, 75, 99, 140, 150 };
                        promoData = new string[] { "150 MB/day FunALIW apps + 300 MB", "150 MB/day FunALIW apps + 750 MB", "6GB of data", "8GB of data", "16GB of data", "18 of data", "30GB of data" };
                        promoFreebies = new string[] { "Unli AllNet Calls & Texts", "Unli AllNet Texts", "Unli AllNet Calls & Texts", "Unli AllNet Call & Texts", "Unli AllNet Texts", "Unli AllNet Calls & Texts", "Unli AllNet Calls & Texts" };
                        promoDays = new int[] { 2, 2, 3, 3, 7, 7, 15 };
                        break;
                    case 2:
                        promoNames = new string[] { "GoUNLI30", "Go59 for Students", "GoEXTRA59", "GoPLUS99", "GoPLUS129", "GoPLUS250" };
                        promoPrices = new int[] { 30, 59, 59, 99, 129, 250 };
                        promoData = new string[] { "100 MB", "5GB plus 1GB for GoLEARN apps", "5GB of data", "16GB of data", "18GB of data", "30GB of data" };
                        promoFreebies = new string[] { "Unli AllNet Texts", "Unli AllNet Texts", "Unli AllNet Calls & Texts", "Unli AllNet Texts", "Unli AllNet Texts & Unli Calls to Globe and TM", "15GB for choice of apps" };
                        promoDays = new int[] { 2, 3, 3, 7, 7, 15 };
                        break;
                    case 3:
                        promoNames = new string[] { "SURFSAYA 35", "POWER ALL 59", "NON-STOP DATA 85", "SAYA ALL 99", "NON-STOP DATA 115", "POWER ALL FB 149", "MAGIC DATA 349" };
                        promoPrices = new int[] { 35, 59, 85, 99, 115, 149, 349 };
                        promoData = new string[] { "500MB ARAW-ARAW for Tiktok, IG, FB, ML + 900MB", "5GB + 3GB 5G of data", "Unli 5G", "6GB + UNLI Tiktok, FB, ML", "Unli 5G + NON-STOP 4G of data", "Unli 5G + 16GB + Unli FB, IG, MEESENGER", "16GB NO EXPIRY DATA" };
                        promoFreebies = new string[] { "Unli AllNet Texts", "Unli AllNet Calls & Texts", "NON-STOP 4G of data", "Unli Calls & Texts", "Unli Calls & Texts", "Unli Calls & Texts", " " };
                        promoDays = new int[] { 3, 3, 3, 7, 3, 7, 0 };
                        break;
                    case 4:
                        promoNames = new string[] { "SUFSAYA 49", "SAYA ALL 50", "SURFSAYA 99", "ARAW-ARAW DATA 150", "MAGIC DATA 249", "ALL DATA 299" };
                        promoPrices = new int[] { 49, 99, 150, 249, 299 };
                        promoData = new string[] { "1.2GB of data + 250MB/day(FB, ML, IG, TT)", "9GB of data", "100/day(FB, ML, IG, TT) + 1.5GB", "1GB/day(10.5GB)", "8GB NO EXPIRY DATA", "24 GB of data" };
                        promoFreebies = new string[] { "Unli Calls & Texts", "Unli Calls & Texts", "Unli Calls & Texts", "10 Mins Calls to Mobile + 100 Texts", " ", " " };
                        promoDays = new int[] { 3, 7, 7, 7, 0, 30 };
                        break;
                    case 5:
                        promoNames = new string[] { "Level-Up Socials 50", "Level-Up 99 5G", "Level-up 109 5G", "Level-Up 129 5G", "Level-Up 199 5G", "Unli 5G 299" };
                        promoPrices = new int[] { 50, 99, 109, 129, 199, 299 };
                        promoData = new string[] { "7GB of data", "14GB 5G of data", "16GB of data", "20GB of data", "32GB of data", "Unli 5G of data + 10GB High-Speed 4G Data" };
                        promoFreebies = new string[] { " ", "Unli AllNet Text + 300 Mins Unli AllNet Calls + Unli Calls Dito-to-Dito", "Unli AllNet Text + 300 Mins Unli AllNet Calls + Unli Calls Dito-to-Dito", "Unli AllNet Text + 300 Mins Unli AllNet Calls + Unli Calls Dito-to-Dito", "Unli AllNet Text + 300 Mins Unli AllNet Calls + Unli Calls Dito-to-Dito", "Unli AllNet Text + 300 Mins Unli AllNet Calls + Unli Calls Dito-to-Dito" };
                        promoDays = new int[] { 3, 30, 30, 30, 30, 7 };
                        break;
                }

                Console.WriteLine("\nAvailable Promos:");

                for (int i = 0; i < promoNames.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + promoNames[i]);
                }

                Console.Write("Select Promo: ");
                int choice = GetValidMenuChoice(promoNames.Length);

                string name = promoNames[choice - 1];
                int price = promoPrices[choice - 1];
                string data = promoData[choice - 1];
                string freebies = promoFreebies[choice - 1];
                int days = promoDays[choice - 1];

                string displayValidity = (days == 0) ? "No Expiration" : days + " Days";

                Console.WriteLine("\n===== CONFIRM TRANSACTION =====");
                Console.WriteLine("Mobile Number : " + phone + " (" + networkName + ")");
                Console.WriteLine("Promo Name    : " + name);
                Console.WriteLine("Inclusions    : " + data + " + " + freebies);
                Console.WriteLine("Price         : P" + price);
                Console.WriteLine("Validity      : " + displayValidity);
                Console.WriteLine("Payment Via   : Wallet Balance");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. BUY PROMO");
                Console.WriteLine("2. Cancel");
                Console.Write("Choose: ");

                int confirm = GetValidMenuChoice(2);

                if (confirm == 2)
                {
                    Console.WriteLine("Transaction Cancelled.");
                    return;
                }

                if (balance < price)
                {
                    Console.WriteLine("Insufficient Wallet Balance. Please Cash-In first.");
                    return;
                }

                balance -= price;
                int pointsEarned = price / 100;
                totalPoints += pointsEarned;

                string receiptExpiry = "";
                if (days == 0)
                {
                    receiptExpiry = "No expiration";
                }
                else
                {
                    receiptExpiry = DateTime.Now.AddDays(days).ToString("MM/dd/yyyy hh:mm tt");
                }

                if (targetChoice == 1)
                {
                    activePromo = name;
                    activeData = data;
                    activeFreebies = freebies;
                    activeExpiry = receiptExpiry;
                }

                ConfirmationMessage(phone, name, price, data, pointsEarned, receiptExpiry);
                SaveHistory("Registered Promo " + name + " to " + phone);
            }



            static void RedeemPoints()
            {
                Console.WriteLine("\n---- REDEEM REWARD PROMOS ----");
                Console.WriteLine("Current Points: " + totalPoints + " points");
                Console.WriteLine("1. 250MB for 1 Day  (Required Points: 2)");
                Console.WriteLine("2. 500MB for 1 Day  (Required Points: 5)");
                Console.WriteLine("3. 1GB for 1 Day    (Required Points: 10)");
                Console.WriteLine("4. 2GB for 3 Days   (Required Points: 20)");
                Console.WriteLine("5. 5GB for 5 Days   (Required Points: 50)");
                Console.WriteLine("6. 8GB for 7 Days   (Required Points: 100)");
                Console.WriteLine("7. Cancel");
                Console.Write("Select reward: ");

                int choice = GetValidMenuChoice(7);

                int pointsRequired = 0;
                string rewardName = "";
                string rewardData = "";
                int rewardDays = 0;

                switch (choice)
                {
                    case 1:
                        pointsRequired = 2;
                        rewardName = "Data 250MB";
                        rewardData = "250MB";
                        rewardDays = 1;
                        break;
                    case 2:
                        pointsRequired = 5;
                        rewardName = "Data 500MB";
                        rewardData = "500MB";
                        rewardDays = 1; break;
                    case 3:
                        pointsRequired = 10;
                        rewardName = "Data 1GB";
                        rewardData = "1GB";
                        rewardDays = 1;
                        break;
                    case 4:
                        pointsRequired = 20;
                        rewardName = "Data 2GB";
                        rewardData = "2GB";
                        rewardDays = 3;
                        break;
                    case 5:
                        pointsRequired = 50;
                        rewardName = "Data 5GB";
                        rewardData = "5GB";
                        rewardDays = 5;
                        break;
                    case 6:
                        pointsRequired = 100;
                        rewardName = "Data 8GB";
                        rewardData = "8GB";
                        rewardDays = 7;
                        break;
                    case 7:
                        Console.WriteLine("Redemption Cancelled.");
                        return;
                }

                if (totalPoints < pointsRequired)
                {
                    Console.WriteLine("Insufficient points! You need " + pointsRequired + " points to avail this reward.");
                    return;
                }

                totalPoints -= pointsRequired;

                activePromo = rewardName;
                activeData = rewardData;
                activeFreebies = "None";
                activeExpiry = DateTime.Now.AddDays(rewardDays).ToString("MM/dd/yyyy hh:mm tt");

                Console.WriteLine("\n---------------------------------------");
                Console.WriteLine("REWARD REDEEMED SUCCESSFULLY");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Congratulations! You successfully redeemed " + rewardName + ".");
                Console.WriteLine("Enjoy " + rewardData + " valid until " + activeExpiry + ".");
                Console.WriteLine("Remaining Points: " + totalPoints);
                Console.WriteLine("-----------------------------------------");

                SaveHistory("Redeemed Reward Promo: " + rewardName);
            }

            static void CheckBalance()
            {
                Console.WriteLine("\n--- ACCOUNT STATUS ---");
                Console.WriteLine("Current Wallet Balance : Php" + balance);
                Console.WriteLine("SIM Regular Load       : Php" + mySimLoad);
                Console.WriteLine("Total Reward Points    : " + totalPoints + " points");
            }

            static void PromoStatus()
            {
                Console.WriteLine("\nActive Load Status");

                if (activePromo != "None")
                {
                    Console.WriteLine("Data      : " + activeData);
                    Console.WriteLine("Additional  : " + activeFreebies);
                    Console.WriteLine("Valid Until: " + activeExpiry);
                }
            }

            static void ShowHistory()
            {
                Console.WriteLine("\n===== TRANSACTION HISTORY =====");

                if (history.Count == 0)
                {
                    Console.WriteLine("No transactions yet.");
                    return;
                }

                for (int i = 0; i < history.Count; i++)
                {
                    Console.WriteLine(history[i]);
                }
            }

            static void SaveHistory(string message)
            {
                string record = "Transaction " + transactionNumber + " - " + message;
                history.Add(record);
                transactionNumber++;
            }

        static void ConfirmationMessage(string phone, string item, int price, string dataByte, double pointsEarned, string expiryDate)
        {
            Random rnd = new Random();
            int randomRef = rnd.Next(1000000, 9999999);

            Console.WriteLine("\n-----------------------------------------");
            Console.WriteLine("           NEW MESSAGE RECEIVED          ");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

            if (dataByte == "Regular")
            {
                Console.WriteLine("\nThank you! You have successfully loaded Regular " + price + " to " + phone + ". \nValid Until " + expiryDate + ".");
                Console.WriteLine("You earned " + pointsEarned + " points from this transaction. You have a total of " + totalPoints + " points. \nRemaining Wallet Balance: Php" + balance + ".");
                Console.WriteLine("\nRef No: " + transactionNumber + randomRef);
            }
            else
            {
                Console.WriteLine("\nYou have successfully registered " + item + " to " + phone + ".");

                if (expiryDate == "No Expiration")
                {
                    Console.WriteLine("Enjoy " + dataByte + " + " + activeFreebies + ". This promo has " + expiryDate + ".");
                }
                else
                {
                    Console.WriteLine("Enjoy " + dataByte + " + " + activeFreebies + " valid until " + expiryDate + ".");
                }
                Console.WriteLine("Remaining Wallet Balance: Php" + balance + ".");
                Console.WriteLine("\nRef No: " + transactionNumber + randomRef);
                Console.WriteLine("-----------------------------------------");
            }

        }
                static int GetValidMenuChoice(int max)
                {
                    while (true)
                    {
                        string choice = Console.ReadLine();

                        if (choice == null || choice.Length == 0)
                        {
                            Console.Write("Choice cannot be empty. \nChoose:" + "1 - " + max + ": ");
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
                        Console.Write("Invalid. Please input a number. \nChoose:" + "1 - " + max + ": ");
                    }
                }

                static int GetValidNumber()
                {
                    while (true)
                    {
                        string input = Console.ReadLine();

                        if (input == null || input.Length == 0)
                        {
                            Console.Write("Amount cannot be empty. Please try again, \nEnter amount: ");
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
                        Console.Write("Invalid. Please input a number. \nEnter amount: ");
                    }
                }
           
      }
}

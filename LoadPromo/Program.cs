using System;
using System.Collections.Generic;


namespace LoadPromos
{
    internal class Program
    {
        static int balance = 0;
        static int totalPoints = 0;

        static string ownerNumber = "";
        static string ownerNetwork = "";
        static int ownerNetworkID = 0;

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
                Console.WriteLine("\n====== DASHBOARD ======");
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

            static void RegisterDefaultAccount()
            {
                Console.WriteLine("-----Load Promo System-----");

                Console.WriteLine("Select your default SIM Network: ");
                ownerNetworkID = SelectNetwork();
                ownerNetwork = NetworkName(ownerNetworkID);

                Console.WriteLine("Enter your default mobile number.");
                ownerNumber = GetValidPhoneNumber();

                Console.WriteLine("\nRegistration Successful!");
                Console.WriteLine("Default Network : " + ownerNetwork);
                Console.WriteLine("Default Number  : " + ownerNumber);
                Console.WriteLine("Press Enter to Procced");
                Console.ReadLine();
            }

            static string NetworkName(int choice)
            {
                string[] networks = {"TM", "Globe", "Smart", "TNT", "Dito"};
                return networks[choice - 1];
            }

            static int SelectNetwork()
            {
                List<string> networks = new List<string> {"TM", "Globe", "Smart", "TNT", "Dito"};

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

                    if (number == null || number.Length == 0) {
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
                        Console.WriteLine("Invalid format. Please try again.");
                }
            }

            static void TopUp()
            {
                Console.Write("Enter amount to Cash-In: ");
                int amount = GetValidNumber();

                if (amount < 1)
                {
                    Console.WriteLine("Minimum Cash-in is Php 1.");
                    return;
                }

                balance += amount;
                Console.WriteLine("Wallet topped up successfully!");
                Console.WriteLine("New Wallet Balance: Php " + balance);

                SaveHistory("Cashed-in Php " + amount + " to wallet.");
            }

            static void RegularLoad()
            {
                Console.WriteLine("\n----- Regular Load -----");
                Console.WriteLine("Select Recipient:");
                Console.WriteLine("1. My number (" + ownerNetwork + " - " + ownerNumber);
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

                int[] loads = {10, 20, 30, 50, 70, 100, 200, 300, 500};

                Console.WriteLine("\nRegular Loads Available");

                for (int i = 0; i < loads.Length; i++)
                {
                    Console.WriteLine("Regular " + loads[i]);
                }

                Console.Write("Select: ");
                int choice = GetValidMenuChoice(loads.Length);

                int amount = loads[choice - 1];

                Console.WriteLine("\n==== CONFIRM TRANSACTION ====");
                Console.WriteLine("Mobile Number : " + phone);
                Console.WriteLine("Load          : " + "Regular " + amount);
                Console.WriteLine("Amount        : " + "Php " + amount);
                Console.WriteLine("Payment Via   : " + "Wallet Balance");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. BUY LOAD");
                Console.WriteLine("2. Cancel");
                Console.WriteLine("Choose: ");
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
                int pointsEarned = amount / 100;
                totalPoints += pointsEarned;

                ConfirmationMessage(phone, "Regular Load", amount, "Regular", pointsEarned);
                SaveHistory("Regular Load Php " + amount + "to " + phone);

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
                    case 7: Console.WriteLine("Redemption Cancelled."); 
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

            static void ConfirmationMessage(string phone, string item, int price, string dataByte, int pointsEarned)
            {
                Random rnd = new Random();
                int randomRef = rnd.Next(1000000000, 999999999);

                Console.WriteLine("\n-----------------------------------------");
                Console.WriteLine("📩 NEW MESSAGE RECEIVED");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Date: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));

                if (dataByte == "Regular")
                {
                    Console.WriteLine("\nThank you! You have successfully loaded Php " + price + " to " + phone + ".");
                }
                else
                {
                    Console.WriteLine("\nYou have successfully loaded " + item + " to " + phone + ".");
                    Console.WriteLine("Enjoy " + dataByte + " + " + activeFreebies + " valid until " + activeExpiry + ".");
                }

                Console.WriteLine("\nRef No: " + transactionNumber + randomRef);
                Console.WriteLine("Remaining Wallet Balance: P" + balance);
                Console.WriteLine("Points Earned: " + pointsEarned);
                Console.WriteLine("Total Points: " + totalPoints);
                Console.WriteLine("-----------------------------------------");
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

               
                //static void PromoLoad(int simChoice)
                //{
                //    Console.WriteLine("\nAvailable Promos:");

                //    if (simChoice == 1)
                //    {
                //        Console.WriteLine("1. TM Easy50 - ₱50 | 2GB");
                //        Console.WriteLine("2. TM Combo99 - ₱99 | 8GB");
                //    }
                //    else if (simChoice == 2)
                //    {
                //        Console.WriteLine("1. Go50 - ₱50 | 5GB");
                //        Console.WriteLine("2. Go99 - ₱99 | 8GB");
                //        Console.WriteLine("3. Go120 - ₱120 | 10GB");
                //    }
                //    else if (simChoice == 3)
                //    {
                //        Console.WriteLine("1. Giga50 - ₱50 | 4GB");
                //        Console.WriteLine("2. Giga99 - ₱99 | 8GB");
                //    }
                //    else if (simChoice == 4)
                //    {
                //        Console.WriteLine("1. TNT50 - ₱50 | 3GB");
                //        Console.WriteLine("2. TNT99 - ₱99 | 7GB");
                //    }
                //    else if (simChoice == 5)
                //    {
                //        Console.WriteLine("1. DITO50 - ₱50 | 5GB");
                //        Console.WriteLine("2. DITO99 - ₱99 | 10GB");
                //    }

                //    Console.Write("Select promo: ");
                //    int choice = Convert.ToInt32(Console.ReadLine());

                //    int price = 0;
                //    string name = "";
                //    int mb = 0;


                //    if (simChoice == 1)
                //    {
                //        if (choice == 1) { name = "TM Easy50"; price = 50; mb = 2048; }
                //        else if (choice == 2) { name = "TM Combo99"; price = 99; mb = 8000; }
                //    }

                //    else if (simChoice == 2)
                //    {
                //        if (choice == 1) { name = "Go50"; price = 50; mb = 5000; }
                //        else if (choice == 2) { name = "Go99"; price = 99; mb = 8000; }
                //        else if (choice == 3) { name = "Go120"; price = 120; mb = 10000; }
                //    }

                //    else if (simChoice == 3)
                //    {
                //        if (choice == 1) { name = "Giga50"; price = 50; mb = 4000; }
                //        else if (choice == 2) { name = "Giga99"; price = 99; mb = 8000; }
                //    }

                //    else if (simChoice == 4)
                //    {
                //        if (choice == 1) { name = "TNT50"; price = 50; mb = 3000; }
                //        else if (choice == 2) { name = "TNT99"; price = 99; mb = 7000; }
                //    }

                //    else if (simChoice == 5)
                //    {
                //        if (choice == 1) { name = "DITO50"; price = 50; mb = 5000; }
                //        else if (choice == 2) { name = "DITO99"; price = 99; mb = 10000; }
                //    }

                //    int points = price / 10;
                //    totalPoints += points;

                //    Console.WriteLine($"Promo applied: {name}");
                //    Console.WriteLine($"Data received: {mb}MB");
                //    Console.WriteLine($"Points earned: {points}");
                //    Console.WriteLine($"otal Points: {totalPoints}");
            }
    }
}

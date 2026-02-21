using System;

namespace LoadPromos
{
    internal class Program
    {
        static int totalPoints = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Select SIM Handler:");
            Console.WriteLine("1. TM");
            Console.WriteLine("2. Globe");
            Console.WriteLine("3. Smart");
            Console.WriteLine("4. TNT");
            Console.WriteLine("5. DITO");
            Console.Write("Enter choice: ");

            int simChoice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n1. Regular Load");
            Console.WriteLine("2. Promos");
            Console.Write("Choose: ");
            int menuChoice = Convert.ToInt32(Console.ReadLine());

            if (menuChoice == 1)
                RegularLoad();
            else
                PromoLoad(simChoice);
        }


        static void RegularLoad()
        {
            Console.WriteLine("\nRegular Loads:");
            Console.WriteLine("1. ₱10");
            Console.WriteLine("2. ₱20");
            Console.WriteLine("3. ₱50");
            Console.WriteLine("4. ₱100");
            Console.Write("Select load: ");

            int choice = Convert.ToInt32(Console.ReadLine());
            int amount = 0;

            if (choice == 1) amount = 10;
            else if (choice == 2) amount = 20;
            else if (choice == 3) amount = 50;
            else if (choice == 4) amount = 100;

            int points = amount / 10;
            totalPoints += points;

            Console.WriteLine($" Loaded ₱{amount}");
            Console.WriteLine($" Points earned: {points}");
            Console.WriteLine($" Total Points: {totalPoints}");
        }


        static void PromoLoad(int simChoice)
        {
            Console.WriteLine("\nAvailable Promos:");

            if (simChoice == 1)
            {
                Console.WriteLine("1. TM Easy50 - ₱50 | 2GB");
                Console.WriteLine("2. TM Combo99 - ₱99 | 8GB");
            }
            else if (simChoice == 2)
            {
                Console.WriteLine("1. Go50 - ₱50 | 5GB");
                Console.WriteLine("2. Go99 - ₱99 | 8GB");
                Console.WriteLine("3. Go120 - ₱120 | 10GB");
            }
            else if (simChoice == 3)
            {
                Console.WriteLine("1. Giga50 - ₱50 | 4GB");
                Console.WriteLine("2. Giga99 - ₱99 | 8GB");
            }
            else if (simChoice == 4)
            {
                Console.WriteLine("1. TNT50 - ₱50 | 3GB");
                Console.WriteLine("2. TNT99 - ₱99 | 7GB");
            }
            else if (simChoice == 5)
            {
                Console.WriteLine("1. DITO50 - ₱50 | 5GB");
                Console.WriteLine("2. DITO99 - ₱99 | 10GB");
            }

            Console.Write("Select promo: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            int price = 0;
            string name = "";
            int mb = 0;


            if (simChoice == 1)
            {
                if (choice == 1) { name = "TM Easy50"; price = 50; mb = 2048; }
                else if (choice == 2) { name = "TM Combo99"; price = 99; mb = 8000; }
            }

            else if (simChoice == 2)
            {
                if (choice == 1) { name = "Go50"; price = 50; mb = 5000; }
                else if (choice == 2) { name = "Go99"; price = 99; mb = 8000; }
                else if (choice == 3) { name = "Go120"; price = 120; mb = 10000; }
            }

            else if (simChoice == 3)
            {
                if (choice == 1) { name = "Giga50"; price = 50; mb = 4000; }
                else if (choice == 2) { name = "Giga99"; price = 99; mb = 8000; }
            }

            else if (simChoice == 4)
            {
                if (choice == 1) { name = "TNT50"; price = 50; mb = 3000; }
                else if (choice == 2) { name = "TNT99"; price = 99; mb = 7000; }
            }

            else if (simChoice == 5)
            {
                if (choice == 1) { name = "DITO50"; price = 50; mb = 5000; }
                else if (choice == 2) { name = "DITO99"; price = 99; mb = 10000; }
            }

            int points = price / 10;
            totalPoints += points;

            Console.WriteLine($"Promo applied: {name}");
            Console.WriteLine($"Data received: {mb}MB");
            Console.WriteLine($"Points earned: {points}");
            Console.WriteLine($"otal Points: {totalPoints}");
        }
    }
}

using System;
using System.Collections.Generic;
using LoadPromoDataService;
using LoadPromoModels;

namespace LoadPromoAppService
{
    public class TransactionService()
    {
        public AccountHolder accHolder = new AccountHolder();
        public TransactionHolder transacHolder = new TransactionHolder();
        public PromoHolder promoHolder = new PromoHolder();
        public NetworkHolder netHolder = new NetworkHolder();
        public LoadOptionsHolder regLoadHolder = new LoadOptionsHolder();
        public Random rnd = new Random();

        public string GenerateRef()
        {
            return rnd.Next(1000000, 9999999).ToString();
        }

        public void RegisterUser(string network, int networkId, string phone)
        {
            Account myAccount = accHolder.GetAccount();
            myAccount.Network = network;
            myAccount.NetworkID = networkId;
            myAccount.PhoneNumber = phone;

            accHolder.UpdateAccount(myAccount);
        }

        public Account GetMyAccount()
        {
            return accHolder.GetAccount();
        }

        public List<Transaction> GetHistory()
        {
            return transacHolder.GetAll();
        }
        public string TopUp (int amount)
        {
            if (amount < 5)
                return "Minimum Cash-in is Php 5.";

            Account acc = accHolder.GetAccount();
            acc.WalletBalance += amount;
            accHolder.UpdateAccount(acc);

            Transaction t = new Transaction
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = GenerateRef(),
                Details = "Cashed-In Php " + amount + " to Wallet"
            };
            transacHolder.Add(t);

            return "Wallet topped up successfully! \nNew Balance: Php " + acc.WalletBalance;
        }

        public string BuyRegularLoad(int amount, string userPhone, bool isMyNumber)
        {
            Account acc = accHolder.GetAccount();
            if (acc.WalletBalance < amount)
                return "Insufficient Wallet Balance. Please Top-Up first.";

            acc.WalletBalance -= amount;

            double pointsEarned = amount / 100.0;
            acc.TotalPoints = pointsEarned;

            if (isMyNumber)
                acc.MySimLoad += amount;
            accHolder.UpdateAccount(acc);

            string expiry = DateTime.Now.AddDays(365).ToString("MM/dd/yyyy hh:mm tt");
            string refNo = GenerateRef();

            Transaction t = new Transaction
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = refNo,
                Details = "Loaded Regular " + amount + " to 0" + userPhone
            };
            transacHolder.Add(t);

            return
                $"\n-----------------------------------------\n" +
                $"           NEW MESSAGE RECEIVED          \n" +
                $"-----------------------------------------" +
                $"\n{t.Date}\n\nYou have successfully loaded Regular P{amount} to 0{userPhone}. Valid Until {expiry}.\nYou earned {pointsEarned} point(s) from this transaction. You have a total of {acc.TotalPoints} point(s).\nRemaining Load Balance: Php {acc.WalletBalance} \n\nRef No. {refNo}\n-----------------------------------------";
        }
        public string BuyPromoLoad(PromoItem promo, string userPhone, bool isMyNumber)
        {
            Account acc = accHolder.GetAccount();

            if (acc.WalletBalance < promo.Price)
                return "Insufficient Load Balance. Please Top-Up first to avail this promo.";

            acc.WalletBalance -= promo.Price;

            string expiry = DateTime.Now.AddDays(promo.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");
            if (isMyNumber)
            {
                acc.ActivePromo = promo.Name;
                acc.ActiveData = promo.DataAllowance;
                acc.ActiveFreebies = promo.Freebies;
                acc.ActiveExpiry = expiry;
            }

            accHolder.UpdateAccount(acc);
            string refNo = GenerateRef();

            Transaction t = new Transaction
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = refNo,
                Details = "Registered " + promo.Name + " to 0" + userPhone 
            };
            transacHolder.Add(t);

            return
               $"\n-----------------------------------------\n" +
               $"           NEW MESSAGE RECEIVED          \n" +
               $"-----------------------------------------" +
               $"\n{t.Date}\n\nYou have successfully registered {promo.Name} to 0{userPhone}. \nEnjoy {promo.DataAllowance} + {promo.Freebies}. \nValid until{expiry}. Remaining Load Balance: Php {acc.WalletBalance} \n\nRef No. {refNo}\n-----------------------------------------";
        }

        public string RedeemPoints(PromoItem reward, int pointsCost)
        {
            Account acc = accHolder.GetAccount();

            if (acc.TotalPoints < pointsCost)
            {
                return "Transaction Failed: Insufficient points to redeem this reward.";
            }

            acc.TotalPoints -= pointsCost;

            string expiry = DateTime.Now.AddDays(reward.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");

            acc.ActivePromo = reward.Name;
            acc.ActiveData = reward.DataAllowance;
            acc.ActiveExpiry = expiry;

            accHolder.UpdateAccount(acc);

            string refNo = GenerateRef();
            Transaction t = new Transaction 
            { 
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = refNo, 
                Details = "Redeemed Reward: " + reward.Name 
            };
            transacHolder.Add(t);

            return 
                $"\nCongratultions! You successfully redeemed {reward.Name} using {pointsCost} points!\nEnjoy {reward.DataAllowance}, valid until {expiry}.\nRemaining Points: {acc.TotalPoints}";
        }

        public List<string> GetNetworks()
        {
            return netHolder.GetAllNetworks();
        }
        public List<int> GetRegularLoadOptions()
        {
            return regLoadHolder.GetRegLoadOptions();
        }
        public List<PromoItem> GetPromos(int networkId)
        {
            return promoHolder.GetPromosByNetwork(networkId);
        }
        public List<PromoItem> GetRewards()
        {
            return promoHolder.GetRewards();
        }

    }
}

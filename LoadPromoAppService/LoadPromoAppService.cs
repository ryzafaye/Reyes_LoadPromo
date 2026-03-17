using System;
using System.Collections.Generic;
using LoadPromoDataService;
using LoadPromoModels;

namespace LoadPromoAppService
{
    public class TransactionService
    {
        private AccountHolder accHolder = new AccountHolder();
        private TransactionHolder transacHolder = new TransactionHolder();
        private PromoHolder promoHolder = new PromoHolder();
        private NetworkHolder netHolder = new NetworkHolder();
        private LoadOptionsHolder regLoadHolder = new LoadOptionsHolder();
        private Random rnd = new Random();

        public string GenerateRef()
        {
            return rnd.Next(1000000, 9999999).ToString();
        }

        public void RegisterUser(string network, int networkId, string phone)
        {
            Account acc = accHolder.GetAccount();
            acc.Network = network;
            acc.NetworkID = networkId;
            acc.PhoneNumber = phone;

            accHolder.UpdateAccount(acc);
        }

        public Account GetMyAccount()
        {
            return accHolder.GetAccount();
        }

        public List<Transaction> GetHistory()
        {
            return transacHolder.GetAll();
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

        public TransactionResponse TopUp (int amount)
        {
            if (amount < 5)
                return new TransactionResponse 
            { 
                 ResultStatus = Status.InvalidAmount 
            };

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

            return new TransactionResponse 
            { 
                ResultStatus = Status.Success, ReceiptData = t 
            };
        }

        public TransactionResponse BuyRegularLoad(int amount, string userPhone, bool isMyNumber)
        {
            Account acc = accHolder.GetAccount();
            if (acc.WalletBalance < amount)
                return new TransactionResponse
                {
                    ResultStatus = Status.InsufficientBalance
                };

            acc.WalletBalance -= amount;
            double pointsEarned = amount / 100.0;
            acc.TotalPoints += pointsEarned;

            if (isMyNumber) acc.MySimLoad += amount;
            accHolder.UpdateAccount(acc);

            Transaction t = new Transaction
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = GenerateRef(),
                Details = "Loaded Regular " + amount + " to 0" + userPhone
            };
            transacHolder.Add(t);

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t};
        }
        public TransactionResponse BuyPromoLoad(PromoItem promo, string userPhone, bool isMyNumber)
        {
            Account acc = accHolder.GetAccount();

            if (acc.WalletBalance < promo.Price)
                return new TransactionResponse
                {
                    ResultStatus = Status.InsufficientBalance
                };

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

            Transaction t = new Transaction
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = GenerateRef(),
                Details = "Registered " + promo.Name + " to 0" + userPhone
            };
            transacHolder.Add(t);

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t};
        }

        public TransactionResponse RedeemPoints(PromoItem reward, int pointsCost)
        {
            Account acc = accHolder.GetAccount();

            if (acc.TotalPoints < pointsCost)
                return new TransactionResponse
                {
                    ResultStatus = Status.InsufficientPoints
                };

            acc.TotalPoints -= pointsCost;
            string expiry = DateTime.Now.AddDays(reward.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");

            acc.ActivePromo = reward.Name;
            acc.ActiveData = reward.DataAllowance;
            acc.ActiveExpiry = expiry;
            accHolder.UpdateAccount(acc);

            Transaction t = new Transaction 
            { 
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                RefNumber = GenerateRef(), 
                Details = "Redeemed Reward: " + reward.Name 
            };
            transacHolder.Add(t);

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t };

        }


    }
}

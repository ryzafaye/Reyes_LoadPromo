using System;
using System.Collections.Generic;
using LoadPromoDataService;
using LoadPromoModels;

namespace LoadPromoAppService
{
    public class TransactionService
    {      
        AccountDataService accountService = new AccountDataService(new LoadPromoAccountDB());
        TransactionDataService transacService = new TransactionDataService(new LoadPromoTransactionDB());

        private LoadPromoData loadPromoData = new LoadPromoData();
        private Account _currentAccount;
        public TransactionService()
        {
            _currentAccount = null;
        }

        public Account GetMyAccount() 
        {
            if (_currentAccount != null)
            {
                Account acc = accountService.GetByPhone(_currentAccount.PhoneNumber);

                if (acc != null)
                {
                    CheckExpirations(acc);
                    _currentAccount = acc;
                }
                return acc;
            }
            return null;
        }

        public bool LoadAccountIfExists(string phone)
        {
            var acc = accountService.GetByPhone(phone); 
            if (acc != null)
            {
                _currentAccount = acc; 
                return true;
            }
            return false; 
        }

        public void RegisterUser(string network, string phone)
        {
            _currentAccount = new Account
            {
                Network = network,
                PhoneNumber = phone,
            };
            accountService.Add(_currentAccount);
        }   

        public List<Transaction> GetHistory()
        {
            return transacService.GetAll()
            .Where(t => t.PhoneNumber == _currentAccount.PhoneNumber)
            .ToList();
        }

        public List<string> GetNetworks()
        {
            return loadPromoData.GetAllNetworks();
        }

        public List<double> GetRegularLoadOptions()
        {
            return loadPromoData.GetRegLoadOptions();
        }

        public List<PromoItem> GetPromos(string networkName)
        {
            return loadPromoData.GetPromosByNetwork(networkName);
        }

        public TransactionResponse TopUp(double amount)
        {
            if (amount < 5)
                return new TransactionResponse 
            { 
                 ResultStatus = Status.InvalidAmount 
            };

            _currentAccount.WalletBalance += amount;
            accountService.UpdateAccount(_currentAccount);

            Transaction t = RecordTransaction("Topped-Up Wallet", amount, "");

            return new TransactionResponse 
            { 
                ResultStatus = Status.Success, ReceiptData = t 
            };
        }  

        public TransactionResponse BuyRegularLoad(double amount)
        {
            if (_currentAccount.WalletBalance < amount)
                return new TransactionResponse { ResultStatus = Status.InsufficientBalance };

            _currentAccount.WalletBalance -= amount;
            _currentAccount.SimLoadBalance += amount;
            _currentAccount.SimLoadExpiry = DateTime.Now.AddDays(365).ToString("MM/dd/yyyy hh:mm tt");

            accountService.UpdateAccount(_currentAccount);

            Transaction t = RecordTransaction($"Regular Load {amount}", amount, "Wallet Balance");

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t };
        }

        public TransactionResponse BuyPromoLoad(PromoItem promo, int paymentOption)
        {
            if (paymentOption == 1) 
            {
                if (_currentAccount.WalletBalance < promo.Price)
                    return new TransactionResponse { ResultStatus = Status.InsufficientBalance };

                _currentAccount.WalletBalance -= promo.Price;
            }
            else if (paymentOption == 2) 
            {
                if (_currentAccount.SimLoadBalance < promo.Price)
                    return new TransactionResponse { ResultStatus = Status.InsufficientBalance };

                _currentAccount.SimLoadBalance -= promo.Price;
            }

                _currentAccount.ActivePromo = promo.Name;
                _currentAccount.ActiveData = promo.DataAllowance;
                _currentAccount.ActiveFreebies = promo.Freebies;
                _currentAccount.ActiveExpiry = DateTime.Now.AddDays(promo.ValidityDays).ToString("MM/dd/yyyy hh:mm tt");

            accountService.UpdateAccount(_currentAccount);

            string paymentText = (paymentOption == 1) ? "Wallet Balance" : "Load Balance";
            Transaction t = RecordTransaction(promo.Name, promo.Price, paymentText);

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t};
        }

        private void CheckExpirations(Account acc)
        {
            bool needsUpdate = false;
            DateTime now = DateTime.Now;

            if (!string.IsNullOrEmpty(acc.ActiveExpiry) && DateTime.TryParse(acc.ActiveExpiry, out DateTime promoDate))
            {
                if (now > promoDate)
                {
                    acc.ActivePromo = "";
                    acc.ActiveData = "";
                    acc.ActiveFreebies = "";
                    acc.ActiveExpiry = "";
                    needsUpdate = true;
                }
            }

            if (!string.IsNullOrEmpty(acc.SimLoadExpiry) && DateTime.TryParse(acc.SimLoadExpiry, out DateTime regLoadDate))
            {
                if (now > regLoadDate)
                {
                    acc.SimLoadBalance = 0;
                    acc.SimLoadExpiry = "";
                    needsUpdate = true;
                }
            }
            if (needsUpdate)
            {
                accountService.UpdateAccount(acc);
            }
        }

        public TransactionResponse CancelActivePromo()
        {
            string canceledPromo = _currentAccount.ActivePromo;

            _currentAccount.ActivePromo = string.Empty;
            _currentAccount.ActiveData = string.Empty;
            _currentAccount.ActiveFreebies = string.Empty;
            _currentAccount.ActiveExpiry = string.Empty;

            accountService.UpdateAccount(_currentAccount);

            Transaction t = RecordTransaction($"Unsubscribed from {canceledPromo}", 0, " ");

            return new TransactionResponse { ResultStatus = Status.Success, ReceiptData = t };
        }

        public string GenerateRef()
        {
            string refNumber = Guid.NewGuid().ToString("N").Substring(0, 15);
            return refNumber;
        }

        private Transaction RecordTransaction (string loadType, double amount, string paymentMethod) 
        {
            Transaction t = new Transaction
            {
                ReferenceNumber = GenerateRef(),
                Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"),
                PhoneNumber = _currentAccount.PhoneNumber,
                LoadType = loadType,
                Amount = amount,
                PaymentMethod = paymentMethod
            };
            transacService.Add(t);
            return t;
        }
    }
}

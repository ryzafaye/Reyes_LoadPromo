using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public class InMemoryAccountHolder : IAccountHolder
    {
        private List<Account> _account = new List<Account>();

        public void Add(Account acc)
        {
            _account.Add(acc);
        }

        public List<Account> GetAccount()
        { 
            return _account; 
        }

        public Account GetByPhone(string phone)
        {
            return _account.FirstOrDefault(a => a.PhoneNumber == phone);
        }

        public void UpdateAccount(Account acc)
        {
            var existing = GetByPhone(acc.PhoneNumber);
            if (existing != null)
            {
                existing.WalletBalance = acc.WalletBalance;
                existing.SimLoadBalance = acc.SimLoadBalance;
                existing.SimLoadExpiry = acc.SimLoadExpiry;
                existing.ActivePromo = acc.ActivePromo;
                existing.ActiveData = acc.ActiveData;
                existing.ActiveFreebies = acc.ActiveFreebies;
                existing.ActiveExpiry = acc.ActiveExpiry;
            }
        }
    }

    public class InMemoryTransactionHolder : ITransactionHolder
    {
        private List<Transaction> _transactions = new List<Transaction>();
        public void Add(Transaction t)
        {
            _transactions.Add(t);
        }
        public List<Transaction> GetAll()
        {
            return _transactions;
        }
    }
}

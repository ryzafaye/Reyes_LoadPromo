using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public class AccountDataService
    {
        private IAccountHolder _data;
        public AccountDataService(IAccountHolder accountService)
        {
            _data = accountService;
        }

        public void Add(Account acc) 
        { 
            _data.Add(acc); 
        }

        public List<Account> GetAccount()
        {
           return _data.GetAccount();
        }

        public Account GetByPhone(string phone)
        {
            return _data.GetByPhone(phone);
        }

        public void UpdateAccount(Account acc)
        {
            _data.UpdateAccount(acc);
        }
    }

    public class TransactionDataService
    {
        private ITransactionHolder _data;

        public TransactionDataService(ITransactionHolder transacService)
        {
            _data = transacService;
        }

        public void Add(Transaction t)
        {
            _data.Add(t);
        }

        public List<Transaction> GetAll()
        {
            return _data.GetAll();
        }
    }
}

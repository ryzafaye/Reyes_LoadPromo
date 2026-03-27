using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public interface IAccountHolder
    {
        void Add(Account acc);
        List<Account> GetAccount();
        Account GetByPhone(string phone);
        void UpdateAccount(Account acc);
    }

    public interface ITransactionHolder
    {
        void Add(Transaction t);
        List<Transaction> GetAll();
    }
}

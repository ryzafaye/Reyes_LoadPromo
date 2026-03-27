using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public class JsonAccountHolder : IAccountHolder
    {
        private List<Account> _account = new List<Account>();
        private string _jsonFileName;

        public JsonAccountHolder()
        {
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/Accounts.json";

            RetrieveDataFromJsonFile();
        }

        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.Create(_jsonFileName))
            {
                JsonSerializer.Serialize<List<Account>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { Indented = true })
                    , _account);
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName))
            {
                _account = new List<Account>();
                SaveDataToJsonFile();
                return;
            }
                using (var jsonFileReader = File.OpenText(this._jsonFileName))
            {
                this._account = JsonSerializer.Deserialize<List<Account>>
                    (jsonFileReader.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true }) 
                    ?.ToList()?? new List<Account>();
            }
        }

        public void Add(Account account)
        {
            RetrieveDataFromJsonFile();
            _account.Add(account);
            SaveDataToJsonFile();
        }
        public List<Account> GetAccount()
        {
            RetrieveDataFromJsonFile();
            return _account;
        }

        public Account GetByPhone(string phone)
        {
            RetrieveDataFromJsonFile();
            return _account.FirstOrDefault(x => x.PhoneNumber == phone);
        }

        public void UpdateAccount(Account acc)
        {
            RetrieveDataFromJsonFile();

            var existingAccount = _account.FirstOrDefault(x => x.PhoneNumber == acc.PhoneNumber);

            if (existingAccount != null)
            {
                existingAccount.Network = acc.Network;
                existingAccount.WalletBalance = acc.WalletBalance;
                existingAccount.SimLoadBalance = acc.SimLoadBalance;
                existingAccount.SimLoadExpiry = acc.SimLoadExpiry;
                existingAccount.ActivePromo = acc.ActivePromo;
                existingAccount.ActiveData = acc.ActiveData;
                existingAccount.ActiveFreebies = acc.ActiveFreebies;
                existingAccount.ActiveExpiry = acc.ActiveExpiry;
                SaveDataToJsonFile();
            }
        }
    }
}

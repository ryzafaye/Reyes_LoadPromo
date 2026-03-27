using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LoadPromoModels;

namespace LoadPromoDataService
{
    public class JsonTransactionHolder : ITransactionHolder
    {
        private List<Transaction> _transactions = new List<Transaction>();
        private string _jsonFileName;

        public JsonTransactionHolder()
        {
            _jsonFileName = $"{AppDomain.CurrentDomain.BaseDirectory}/Transactions.json";
            RetrieveDataFromJsonFile();
        }

        private void SaveDataToJsonFile()
        {
            using (var outputStream = File.Create(_jsonFileName))
            {
                JsonSerializer.Serialize<List<Transaction>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    { SkipValidation = true, Indented = true })
                    , _transactions);
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            if (!File.Exists(_jsonFileName))
            {
                SaveDataToJsonFile();
                return;
            }

            using (var jsonFileReader = File.OpenText(this._jsonFileName))
            {
                this._transactions = JsonSerializer.Deserialize<List<Transaction>>
                    (jsonFileReader.ReadToEnd(), new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true })?.ToList() ??
                    new List<Transaction>();
            }
        }
        public void Add(Transaction t)
        {
            RetrieveDataFromJsonFile();
            _transactions.Add(t);
            SaveDataToJsonFile();
        }

        public List<Transaction> GetAll()
        {
            RetrieveDataFromJsonFile();
            return _transactions;
        }
    }
}

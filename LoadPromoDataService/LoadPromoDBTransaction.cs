using LoadPromoModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoDataService
{
    public class LoadPromoTransactionDB : ITransactionHolder
    {
        private string connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=LoadPromo; Integrated Security=True; TrustServerCertificate=True;";

        private SqlConnection sqlConnection;

        public LoadPromoTransactionDB()
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public void Add(Transaction t)
        {
            var insertStatement = "INSERT INTO Transactions (phone_number, load_type, amount, transaction_date, reference_number, payment_method) VALUES (@Phone, @Details, @Amount, @Date, @Ref, @PayMethod)";
            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

            insertCommand.Parameters.AddWithValue("@Phone", t.PhoneNumber ?? "");
            insertCommand.Parameters.AddWithValue("@Details", t.LoadType ?? "");
            insertCommand.Parameters.AddWithValue("@Amount", t.Amount);
            insertCommand.Parameters.AddWithValue("@Date", t.Date ?? "");
            insertCommand.Parameters.AddWithValue("@Ref", t.ReferenceNumber ?? "");
            insertCommand.Parameters.AddWithValue("@PayMethod", t.PaymentMethod ?? "");

            sqlConnection.Open();
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Transaction> GetAll()
        {
            string selectStatement = "SELECT * FROM Transactions";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();
            var transactions = new List<Transaction>();

            while (reader.Read())
            {
                Transaction t = new Transaction();
                t.PhoneNumber = reader["phone_number"].ToString();
                t.LoadType = reader["load_type"].ToString();
                t.Amount = Convert.ToInt32(reader["amount"]);
                t.Date = reader["transaction_date"].ToString();
                t.ReferenceNumber = reader["reference_number"].ToString();
                t.ReferenceNumber = reader["payment_method"].ToString();

                transactions.Add(t);
            }

            sqlConnection.Close();
            return transactions;
        }
    }
}

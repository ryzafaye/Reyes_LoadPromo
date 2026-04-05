using LoadPromoModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoDataService
{
    public class LoadPromoAccountDB : IAccountHolder
    {
        private string connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog=LoadPromo; Integrated Security=True; TrustServerCertificate=True;";

        private SqlConnection sqlConnection;

        public LoadPromoAccountDB()
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        public void Add(Account account)
        {
            var insertStatement = "INSERT INTO Accounts (phone_number, pin, network, wallet_balance, sim_load_balance, sim_load_expiry, active_promo, active_data, active_freebies, active_expiry) VALUES (@Phone, @Pin, @Net, @Wal, @Load, @LoadExp, @Promo, @Data, @Free, @Exp)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);

            insertCommand.Parameters.AddWithValue("@Phone", account.PhoneNumber ?? "");
            insertCommand.Parameters.AddWithValue("@Pin", account.PIN);
            insertCommand.Parameters.AddWithValue("@Net", account.Network ?? "");
            insertCommand.Parameters.AddWithValue("@Wal", account.WalletBalance);
            insertCommand.Parameters.AddWithValue("@Load", account.SimLoadBalance);
            insertCommand.Parameters.AddWithValue("@LoadExp", account.SimLoadExpiry ?? "");
            insertCommand.Parameters.AddWithValue("@Promo", account.ActivePromo ?? "");
            insertCommand.Parameters.AddWithValue("@Data", account.ActiveData ?? "");
            insertCommand.Parameters.AddWithValue("@Free", account.ActiveFreebies ?? "");
            insertCommand.Parameters.AddWithValue("@Exp", account.ActiveExpiry ?? "");

            sqlConnection.Open();
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public List<Account> GetAccount()
        {
            string selectStatement = "SELECT * FROM Accounts";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            var accounts = new List<Account>();

            while (reader.Read())
            {
                Account account = new Account();
                account.PhoneNumber = reader["phone_number"].ToString();
                account.PIN = reader["PIN"].ToString();
                account.Network = reader["network"].ToString();
                account.WalletBalance = Convert.ToDouble(reader["wallet_balance"]);
                account.SimLoadBalance = Convert.ToDouble(reader["sim_load_balance"]);
                account.SimLoadExpiry = reader["sim_load_expiry"].ToString();
                account.ActivePromo = reader["active_promo"].ToString();
                account.ActiveData = reader["active_data"].ToString();
                account.ActiveFreebies = reader["active_freebies"].ToString();
                account.ActiveExpiry = reader["active_expiry"].ToString();
                accounts.Add(account);
            }

            sqlConnection.Close();
            return accounts;
        }

        public Account GetByPhone(string phone)
        {
            var selectStatement = "SELECT * FROM Accounts WHERE phone_number = @Phone";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);
            selectCommand.Parameters.AddWithValue("@Phone", phone);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            Account account = null;

            if (reader.Read())
            {
                account = new Account();
                account.PhoneNumber = reader["phone_number"].ToString();
                account.PIN = reader["pin"].ToString();
                account.Network = reader["network"].ToString();
                account.WalletBalance = Convert.ToDouble(reader["wallet_balance"]);
                account.SimLoadBalance = Convert.ToDouble(reader["sim_load_balance"]);
                account.SimLoadExpiry = reader["sim_load_expiry"].ToString();
                account.ActivePromo = reader["active_promo"].ToString();
                account.ActiveData = reader["active_data"].ToString();
                account.ActiveFreebies = reader["active_freebies"].ToString();
                account.ActiveExpiry = reader["active_expiry"].ToString();
            }

            sqlConnection.Close();
            return account;
        }

        public void UpdateAccount(Account account)
        {
            sqlConnection.Open();

            var updateStatement = "UPDATE Accounts SET pin = @PIN, network = @Net, wallet_balance = @Wal, sim_load_balance = @Load, sim_load_expiry = @LoadExp, active_promo = @Promo, active_data = @Data, active_freebies = @Free, active_expiry = @Exp WHERE phone_number = @Phone";

            SqlCommand updateCommand = new SqlCommand(updateStatement, sqlConnection);

            updateCommand.Parameters.AddWithValue("@Phone", account.PhoneNumber);
            updateCommand.Parameters.AddWithValue("@Pin", account.PIN);
            updateCommand.Parameters.AddWithValue("@Net", account.Network ?? "");
            updateCommand.Parameters.AddWithValue("@Wal", account.WalletBalance);
            updateCommand.Parameters.AddWithValue("@Load", account.SimLoadBalance);
            updateCommand.Parameters.AddWithValue("@LoadExp", account.SimLoadExpiry ?? "");
            updateCommand.Parameters.AddWithValue("@Promo", account.ActivePromo ?? "");
            updateCommand.Parameters.AddWithValue("@Data", account.ActiveData ?? "");
            updateCommand.Parameters.AddWithValue("@Free", account.ActiveFreebies ?? "");
            updateCommand.Parameters.AddWithValue("@Exp", account.ActiveExpiry ?? "");

            updateCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
    }
}


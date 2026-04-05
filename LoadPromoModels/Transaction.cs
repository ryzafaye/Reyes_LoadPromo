using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoModels
{
    public class Transaction
    {
        public string ReferenceNumber { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string LoadType { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public enum Status
    {
        Success, 
        InvalidAmount, 
        InsufficientBalance,
        Error
    }

    public class TransactionResponse
    {
        public Status ResultStatus { get; set; }
        public Transaction ReceiptData { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadPromoModels
{
    public class Transaction
    {
        public string RefNumber { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public enum Status
    {
        Success, InvalidAmount, InsufficientBalance, InsufficientPoints
    }

    public class TransactionResponse
    {
        public Status ResultStatus { get; set; }
        public Transaction ReceiptData { get; set; }
    }
}

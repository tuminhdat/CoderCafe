using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Payment
    {
        public int Id { get; set; }
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public Payment()
        {

        }

        public enum PaymentType
        {
            Cash,
            Debit,
            Credit,
            Point
        }
    }
}

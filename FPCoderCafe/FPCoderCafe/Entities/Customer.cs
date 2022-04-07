using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Customer
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string RedeemPoint { get; set; }
        public string BarCode { get; set; }
        public bool IsEnable { get; set; }
        public Customer()
        {
        }
    }
}

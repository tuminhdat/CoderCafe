using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Order
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int OrderNumber { get; set; }
        public string Description { get; set; }
        public List<Item> Items { get; set; }
        public Customer Customer { get; set; }
        public List<Payment> Payments { get; set; }
        public Order()
        {

        }
    }
}

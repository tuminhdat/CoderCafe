using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public List<Item> Items { get; set; } = new List<Item>();
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public List<Payment> Payments { get; set; }
        [NotMapped]
        public decimal TotalAmt { get => Items.Sum(x=>x.Sales); }
        [NotMapped]
        public string CusPhone { get => Customer.Phone; }
        public Order()
        {

        }
    }
}

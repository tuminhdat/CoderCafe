using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Item
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public Size ItemSize { get; set; }
        public string Note { get; set; }
        public Product Product { get; set; }
        public decimal TaxAmount { get; set; }
        public Item()
        {

        }

        public enum Size
        {
            Small,
            Medium,
            Large
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public decimal TaxAmount { get; set; }
       /* [NotMapped]
        public int ItemQuantity { get => Quantity; }
        [NotMapped]
        public decimal ItemPrice { get => Amount; }*/
        [NotMapped]
        public decimal Sales
        {
            get
            {
                decimal sale = Quantity * Amount;
                return sale;
            }
        }
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

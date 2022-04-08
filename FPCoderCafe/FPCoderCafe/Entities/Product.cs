using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Decimal? SmallPrice { get; set; }
        public Decimal? MediumPrice { get; set; }
        public Decimal? LargePrice { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public string ImageName { get; set; }
        public bool IsEnabled { get; set; }
        [NotMapped]
        public string FullImagePath { get => Directory.GetCurrentDirectory() + @"\Images\" + ImageName; }
        public Product()
        {
            IsEnabled = true;
        }
    }
}

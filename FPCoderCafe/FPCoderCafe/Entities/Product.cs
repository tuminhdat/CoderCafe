using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Category Category { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public string FullImagePath { get => "./image/" + ImageName; }
        public Product()
        {

        }
    }
}

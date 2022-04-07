using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPCoderCafe.Entities
{
    class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Product> Products { get; set; }
        public string ImageName { get; set; }

        public bool IsEnable { get; set; }
        [NotMapped]
        //public string FullImagePath { get => @"\Images\" + ImageName; }
        public string FullImagePath { get => Directory.GetCurrentDirectory() + @"\Images\" + ImageName; }
        public Category()
        {
            IsEnable = true;
        }
    }
}

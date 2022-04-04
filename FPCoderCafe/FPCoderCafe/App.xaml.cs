using FPCoderCafe.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FPCoderCafe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() 
        {
            var context = new PointOfSaleContext();            
            context.Database.Migrate();

            bool exists = System.IO.Directory.Exists(Directory.GetCurrentDirectory() + @"/Images/");

            if (!exists)
                System.IO.Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"/Images/");
        }

        
    }
}

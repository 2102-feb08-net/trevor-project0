using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using DAL;

namespace Models
{
    public class SQLStoreApp
    {
        private string _connectionString = File.ReadAllText("C:/revature/project0-connection-string");
        private DbContextOptions<Project0Context> _options;
        public SQLStoreApp()
        {
            _options = new DbContextOptionsBuilder<Project0Context>()
                .UseSqlServer(_connectionString)
                .Options;

        }
        public void Run() 
        {
            using var context = new Project0Context(_options);


        }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class StoreDAL
    {
        public StoreDAL()
        {
            Orders = new HashSet<OrderDAL>();
            StoreItems = new HashSet<StoreItemDAL>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Profit { get; set; }

        public virtual ICollection<OrderDAL> Orders { get; set; }
        public virtual ICollection<StoreItemDAL> StoreItems { get; set; }
    }
}

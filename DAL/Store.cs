using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Store
    {
        public Store()
        {
            Orders = new HashSet<Order>();
            StoreItems = new HashSet<StoreItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Profit { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StoreItem> StoreItems { get; set; }
    }
}

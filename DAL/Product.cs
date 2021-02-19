using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            StoreItems = new HashSet<StoreItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<StoreItem> StoreItems { get; set; }
    }
}

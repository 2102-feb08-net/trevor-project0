using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class ProductDAL
    {
        public ProductDAL()
        {
            OrderItems = new HashSet<OrderItemDAL>();
            StoreItems = new HashSet<StoreItemDAL>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderItemDAL> OrderItems { get; set; }
        public virtual ICollection<StoreItemDAL> StoreItems { get; set; }
    }
}

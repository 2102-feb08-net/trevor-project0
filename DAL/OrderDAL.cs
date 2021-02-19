using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class OrderDAL
    {
        public OrderDAL()
        {
            OrderItems = new HashSet<OrderItemDAL>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }

        public virtual CustomerDAL Customer { get; set; }
        public virtual StoreDAL Store { get; set; }
        public virtual ICollection<OrderItemDAL> OrderItems { get; set; }
    }
}

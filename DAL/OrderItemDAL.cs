using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class OrderItemDAL
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual OrderDAL Order { get; set; }
        public virtual ProductDAL Product { get; set; }
    }
}

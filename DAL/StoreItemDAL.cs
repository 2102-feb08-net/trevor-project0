using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class StoreItemDAL
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual ProductDAL Product { get; set; }
        public virtual StoreDAL Store { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class CustomerDAL
    {
        public CustomerDAL()
        {
            Orders = new HashSet<OrderDAL>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public virtual ICollection<OrderDAL> Orders { get; set; }
    }
}

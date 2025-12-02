using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasEHandel.Models
{
    public class Order
    {
        // PK
        public int OrderId { get; set; }

        // FK
        public int CustomerId { get; set; }

        // Egenskaper
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Tillgång till kundinfo för varje order
        public Customer? Customer { get; set; }

        // En order kan ha flera orderrader
        public List<OrderRow> OrderRows { get; set; } = new();
    }
}

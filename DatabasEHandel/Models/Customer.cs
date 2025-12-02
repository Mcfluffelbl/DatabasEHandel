using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasEHandel.Models
{
    public class Customer
    {
        // PK
        public int CustomerId { get; set; }

        // Egenskaper
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;
        [Required, MaxLength(100)]
        public string City { get; set; } = null!;

        // En kund kan ha flera ordrar
        public List<Order> Orders { get; set; } = new();
    }
}

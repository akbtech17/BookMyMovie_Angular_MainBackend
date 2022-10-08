using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Akbcustomer
    {
        public Akbcustomer()
        {
            Akbtdets = new HashSet<Akbtdet>();
        }

        public int? CustomerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }

        public virtual ICollection<Akbtdet> Akbtdets { get; set; }
    }
}

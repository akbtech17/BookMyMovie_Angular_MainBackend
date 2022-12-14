using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Akbcustomer
    {
        public Akbcustomer()
        {
            AkbtransactionDetails = new HashSet<AkbtransactionDetail>();
        }

        public int? CustomerId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }

        public virtual ICollection<AkbtransactionDetail> AkbtransactionDetails { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Akbtdet
    {
        public int? TransactionId { get; set; }
        public int? CustomerId { get; set; }
        public int? MovieId { get; set; }
        public DateTime TransactionTime { get; set; }
        public int TotalCost { get; set; }

        public virtual Akbcustomer Customer { get; set; }
        public virtual Akbmovie Movie { get; set; }
    }
}

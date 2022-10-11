using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class AkbtransactionSeat
    {
        public int? TransactionId { get; set; }
        public string SeatNo { get; set; }

        public virtual AkbtransactionDetail Transaction { get; set; }
    }
}

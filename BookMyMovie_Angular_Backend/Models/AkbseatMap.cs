using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class AkbseatMap
    {
        public int? SeatId { get; set; }
        public int? MovieId { get; set; }
        public string SeatNo { get; set; }
        public int? Status { get; set; }

        public virtual Akbmovie Movie { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Akbmovie
    {
        public Akbmovie()
        {
            AkbseatMaps = new HashSet<AkbseatMap>();
            Akbtdets = new HashSet<Akbtdet>();
        }

        public int? MovieId { get; set; }
        public string MovieName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? Ratings { get; set; }
        public string Genres { get; set; }
        public string ImageUrl { get; set; }
        public int? CostPerSeat { get; set; }
        public DateTime? ShowTime { get; set; }
        public string Duration { get; set; }
        public string AgeRating { get; set; }
        public string Language { get; set; }
        public string MovieType { get; set; }

        public virtual ICollection<AkbseatMap> AkbseatMaps { get; set; }
        public virtual ICollection<Akbtdet> Akbtdets { get; set; }
    }
}

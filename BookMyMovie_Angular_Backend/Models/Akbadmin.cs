using System;
using System.Collections.Generic;

#nullable disable

namespace BookMyMovie_Angular_Backend.Models
{
    public partial class Akbadmin
    {
        public int? AdminId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
    }
}

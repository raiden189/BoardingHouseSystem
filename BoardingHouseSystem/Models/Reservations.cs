using System;
using System.Collections.Generic;
using System.Text;

namespace BoardingHouseSystem.Models
{
    public class Reservations
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int BoardingId { get; set; }
    }
}

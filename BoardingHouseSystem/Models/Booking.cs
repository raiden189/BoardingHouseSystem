using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardingHouseSystem.Models
{
    public class Booking
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string StudentId { get; set; }

        public string BoardingId { get; set; }

        public string Status { get; set; }

        public Booking() { }

        public Booking(string studentId, string boardingId, string status)
        {
            StudentId = studentId;
            BoardingId = boardingId;
            Status = status;
        }
    }
}

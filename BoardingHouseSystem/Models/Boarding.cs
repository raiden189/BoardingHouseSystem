using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardingHouseSystem.Models
{
    public class Boarding
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public int DistanceRange { get; set; }
        public double MonthlyPayment { get; set; } = 0;
        public int Capacity { get; set; }
        public string Details { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BoardingHouseSystem.Models
{
    public class BoardingHouse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }

        public string ContactNumber { get; set; }

        public string ContactPerson { get; set; }

        public string Details { get; set; }

        public string ThumbNail { get; set; }

        public double MonthlyPayment { get; set; }

        public int DistanceRange { get; set; }

        public IEnumerable<Path> ImagePaths { get; set; }

        public Color ColorDesign { get; set; }
    }

    public class Path
    {
        public string LinkId { get; set; }
        public string ImagePath { get; set; }
    }
}

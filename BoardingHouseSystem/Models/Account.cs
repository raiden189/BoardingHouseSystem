using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BoardingHouseSystem.Models
{
    public class Account
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public bool isStudent { get; set; }

        public string ContactNum { get; set; }

        public string Address { get; set; }

        public string FbAccount { get; set; }

        public string Photo { get; set; }
    }
}

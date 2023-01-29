using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoardingHouseSystem.Models
{
    public class Login
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

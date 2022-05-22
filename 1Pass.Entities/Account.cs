using System;

namespace _1Pass.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public int ServiceId { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}

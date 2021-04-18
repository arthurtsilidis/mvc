using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ClientsExercise.Data.Entities
{
    public partial class Client
    {
        public Client()
        {
            ClientPhones = new HashSet<ClientPhones>();
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public virtual ICollection<ClientPhones> ClientPhones { get; set; }
    }
}

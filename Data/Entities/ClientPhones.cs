using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ClientsExercise.Data.Entities
{
    public partial class ClientPhones
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PhoneTypeId { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Client Client { get; set; }
        public virtual PhoneTypes PhoneType { get; set; }
    }
}

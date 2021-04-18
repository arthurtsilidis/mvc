using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;



// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ClientsExercise.Data.Entities
{
    public partial class PhoneTypes
    {
        public PhoneTypes()
        {
            ClientPhones = new HashSet<ClientPhones>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<ClientPhones> ClientPhones { get; set; }
    }
}

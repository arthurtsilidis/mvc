using ClientsExercise.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsExercise.Models
{
  
    public class Client
    {

public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name should be minimum 2 characters and a maximum of 50 characters")]
        [RegularExpression("^((?!^Last Name$)[a-zA-Z '])+$", ErrorMessage = "Last name is required and must be properly formatted.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First Name should be minimum 2 characters and a maximum of 50 characters")]
        [RegularExpression("^((?!^First Name$)[a-zA-Z '])+$", ErrorMessage = "First name is required and must be properly formatted.")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Address should be minimum 5 characters and a maximum of 100 characters")]
        [RegularExpression("^((?!^Address$)[0-9A-Za-z #.,])+$", ErrorMessage = "Address is required and must be properly formatted.")]
        [DataType(DataType.Text)]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [RegularExpression("^[A-Za-z0-9._%+-]*@[A-Za-z0-9.-]*\\.[A-Za-z0-9-]{2,}$", ErrorMessage = "Email is required and must be properly formatted.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public IList<PhoneNumber> PhoneNumbers { get; set; }
    }
}
